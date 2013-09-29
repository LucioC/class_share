﻿using System;
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
            ImageState = new ImageState();
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

        public event UpdateImageState ImageUpdate;
        public ImageState ImageState { get; set; }
        public void UpdateImageState(ImageState imageState)
        {
            ImageState.Angle = imageState.Angle;
            ImageState.Bottom = imageState.Bottom;
            ImageState.ScreenHeight = imageState.ScreenHeight;
            ImageState.Left = imageState.Left;
            ImageState.Right = imageState.Right;
            ImageState.Top = imageState.Top;
            ImageState.ScreenWidth = imageState.ScreenWidth;
            ImageState.X = imageState.X;
            ImageState.Y = imageState.Y;
            ImageState.Zoom = imageState.Zoom;
            ImageState.Active = imageState.Active;

            if (ImageUpdate != null)
            {
                ImageUpdate.BeginInvoke(ImageState, null, null);
            }
        }

        public event CommandExecutor CommandListeners;
        public void TriggerCommand(String command, String value)
        {
            if (CommandListeners != null)
            {
                CommandListeners.BeginInvoke(command, value, null, null);
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
                CommandListeners -= imageForm.CommandExecutor;
                imageForm.Invoke(new CloseDelegate(imageForm.Close));
            }
        }

        [STAThread]
        public void InitializeForm()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            imageForm = new ImageZoomMainForm(FileName);
            imageForm.AddListenerForUpdateImageEvent(this.UpdateImageState);
            CommandListeners += imageForm.CommandExecutor;
            this.ImageState = imageForm.GetImageState();
            Application.Run(imageForm);
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
