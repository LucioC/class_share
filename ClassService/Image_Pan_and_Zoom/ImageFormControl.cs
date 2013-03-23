using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CommonUtils;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ImageZoom
{
    public class ImageFormControl
    {
        delegate void CloseDelegate();
        private Thread thread;
        public String File { get; set; }
        private ImageZoomMainForm imageForm = null;
        FileManager fileManager;

        public ImageFormControl(String imageFileToOpen)
        {
            File = imageFileToOpen;
            thread = null;
            fileManager = new FileManager();
        }

        //Throws exception if dont exist or cant be converted to an Image
        public void checkIfImageExit()
        {
            if (fileManager.FileExists(File))
            {
                //Better way to do this?
                Image newImage = Image.FromFile(File);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        public void RunFormInNewThread()
        {
            checkIfImageExit();
            thread = new Thread(this.InitializeForm);
            thread.Start();
        }

        public void StopThread()
        {
            if (thread != null && thread.IsAlive && imageForm != null)
            {
                imageForm.Invoke(new CloseDelegate(imageForm.Close));
            }
        }

        [STAThread]
        public void InitializeForm()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(imageForm = new ImageZoomMainForm(File));
        }
    }
}
