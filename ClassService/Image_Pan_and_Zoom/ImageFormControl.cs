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
using System.Globalization;

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
            float zoom = 1f;
            
            if(IsThreadRunning())
            switch (command)
            {
                //TODO move this to another class
                case "visiblepart":
                    int left = 0;
                    int bottom = 0;
                    int right = 0;
                    int top = 0;
                    int imageH = 0;
                    int imageW = 0;
                    int rotation = 0;
                    if (paramArray.Length > 1)
                    {
                        left = Int32.Parse(paramArray[1]);
                        top = Int32.Parse(paramArray[2]);
                        right = Int32.Parse(paramArray[3]);
                        bottom = Int32.Parse(paramArray[4]);
                        imageH = Int32.Parse(paramArray[5]);
                        imageW = Int32.Parse(paramArray[6]);
                        rotation = Int32.Parse(paramArray[7]);
                    }
                    imageForm.Invoke((MethodInvoker)delegate
                    {
                        imageForm.setViewMinimumBounds(left, top, right, bottom, imageH, imageW, rotation);
                    });
                    break;
                case "zoom":
                    if (paramArray.Length > 1)
                    {
                        x = Int32.Parse(paramArray[1]);
                        y = Int32.Parse(paramArray[2]);
                        zoom = float.Parse(paramArray[3], CultureInfo.InvariantCulture);
                    }
                    imageForm.Invoke((MethodInvoker)delegate
                    {
                        imageForm.setPositionAndZoom(x, y, zoom);
                    });
                    break;
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
                case "rotation":
                    int angle = 0;
                    if (paramArray.Length > 1)
                    {
                        angle = Int32.Parse(paramArray[1]);
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
