﻿using System;
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
    public class ImageFormControl : IThreadFileWindow
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
                Image newImage = Image.FromFile(FileName);
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
            Application.Run(imageForm = new ImageZoomMainForm(FileName));
        }

        public void SetFilePath(string fileName)
        {
            FileName = fileName;
        }
    }
}
