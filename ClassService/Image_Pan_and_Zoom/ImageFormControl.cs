using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CommonUtils;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ServiceCore;

namespace ImageZoom
{
    public class ImageFormControl : IImageService
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
        public virtual void CheckIfImageExit()
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
                throw new FileNotFoundException();
            }
        }

        public void StartThread()
        {
            CheckIfImageExit();
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

        public void SendCommand(string command)
        {
            switch (command)
            {
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
