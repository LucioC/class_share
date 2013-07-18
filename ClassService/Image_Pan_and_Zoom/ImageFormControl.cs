using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ServiceCore;
using ServiceCore.Utils;

namespace ImageZoom
{
    public class ImageFormControl : DefaultCommunicator,IImageService
    {
        delegate void CloseDelegate();
        private Thread thread;
        public String FileName { get; set; }
        private ImageZoomMainForm imageForm = null;
        private FileManager fileManager;
        
        public ImageFormControl(String imageFileToOpen)
        {
            FileName = imageFileToOpen;
            thread = null;
            fileManager = new FileManager();
        }

        public ImageFormControl(): this(null)
        {            
        }

        //Throws exception if dont exist or cant be converted to an Image
        public virtual void CheckIfImageExist()
        {
            if (fileManager.FileExists(FileName))
            {
                //Better way to do this?
                //Not using Image.FromFile because it could LOCK image file (dont know why)
                Image newImage;
                using (var bmpTemp = new Bitmap(FileName))
                {
                    newImage = new Bitmap(bmpTemp);
                }
                newImage.Dispose();
            }
            else
            {
                throw new FileNotFoundException("file " + FileName + " was not found on the server.");
            }
        }

        public void StartThread()
        {
            CheckIfImageExist();
            thread = new Thread(this.InitializeForm);
            thread.Start();
        }

        public void StopThread()
        {
            if (IsThreadRunning())
            {
                imageForm.Invoke(new CloseDelegate(imageForm.Close));
            }
        }

        [STAThread]
        public void InitializeForm()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(imageForm = new ImageZoomMainForm(FileName));
        }

        public void SetFilePath(string fileName)
        {
            FileName = fileName;
        }

        public String GetImageFilePath()
        {
            if (this.IsThreadRunning())
            {
                return FileName;
            }
            else
            {
                return null;
            }
        }

        public void SendCommand(string command)
        {
            String[] paramArray = command.Split(Char.Parse(":"));

            command = paramArray[0];

            int x = 0;
            int y = 0;
            switch (command)
            {
                case "move":
                    x = 50;
                    y = 50;
                    if (paramArray.Length > 1)
                    {
                        x = Int32.Parse(paramArray[1]);
                        y = Int32.Parse(paramArray[2]);
                    }
                    imageForm.Invoke((MethodInvoker)delegate
                    {
                        imageForm.addToX(x);
                        imageForm.addToY(y);
                    });
                    break;
                case "zoom":
                    float zoom = 0.1F;
                    Point center = new Point(imageForm.Width/ 2,
                          (imageForm.Height/2));
                    if (paramArray.Length > 1)
                    {
                        zoom = float.Parse(paramArray[1]);
                        if (paramArray.Length > 2)
                        {
                            x = Int32.Parse(paramArray[2]);
                            y = Int32.Parse(paramArray[3]);
                            center = new Point(x, y);
                        }
                    }
                    imageForm.Invoke((MethodInvoker)delegate
                    {
                        imageForm.zoomPicture(zoom, center);
                    });
                    break;
                case "rotation":
                    float angle = 0F;
                    if (paramArray.Length > 1)
                    {
                        angle = float.Parse(paramArray[1]);
                    }
                    imageForm.Invoke((MethodInvoker)delegate
                    {
                        imageForm.setAngle(angle);
                    });
                    break;
                case "moveright":
                    SendKeys.SendWait("{RIGHT}");
                    break;
                case "moveleft":
                    SendKeys.SendWait("{LEFT}");
                    break;
                case "moveup":
                    SendKeys.SendWait("{UP}");
                    break;
                case "movedown":
                    SendKeys.SendWait("{DOWN}");
                    break;
                case "rotateright":
                    SendKeys.SendWait("{END}");
                    break;
                case "rotateleft":
                    SendKeys.SendWait("{HOME}");
                    break;
                case "zoomin":
                    SendKeys.SendWait("{PGUP}");
                    break;
                case "zoomout":
                    SendKeys.SendWait("{PGDN}");
                    break;
                default:
                    throw new ArgumentException("not valid argument passed");
            }
        }
        
        public bool IsThreadRunning()
        {
            if (thread != null && thread.IsAlive && imageForm != null)
            {
                return true;
            }
            return false;
        }
    }
}
