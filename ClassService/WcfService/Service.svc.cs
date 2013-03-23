using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.Office.Interop.PowerPoint;
using PowerPointPresentation;
using System.IO;
using CommonUtils;
using ImageZoom;
using KinectPowerPointControl;

namespace ClassService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Service : IService
    {
        public static PowerPointControl PresentationControl;
        public static ImageFormControl ImageForm;
        private FileManager fileManager;
        public static KinectWindowControl KinectWindow;

        public Service()
        {
            fileManager = new FileManager();
        }

        static Service()
        {        
            KinectWindow = new KinectWindowControl();
            ImageForm = new ImageFormControl();
            PresentationControl = new PowerPointControl();
        }

        public Result PresentationCommand(Action action)
        {
            switch (action.Command)
            {
                case "next": NextSlide();
                    break;
                case "previous": PreviousSlide();
                    break;
                case "close": ClosePresentation();
                    break;
                default: return new Result("wrong command argument option");
            }

            return new Result("Command Executed");
        }
        
        public Result StartPresentation(File file)
        {
            try
            {
                //Get current directory and look for file
                String localPath = Directory.GetCurrentDirectory() + "\\";
                localPath = localPath + file.FileName;

                //Open power point presentation
                PresentationControl.PreparePresentation(localPath);
                PresentationControl.StartPresentation();

                //Initialize Kinect windows for gesture and speech recognition
                KinectWindow.RunWindowInNewThread();

                return new Result("Presentation has been started");
            }
            catch(Exception e)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return new Result("Something went wrong. Verify if the file name is corrected");
            }
        }

        public Result NextSlide()
        {
            try
            {
                PresentationControl.GoToNextSlide();
                return new Result("Advanced one slide");
            }
            catch (Exception e)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return new Result(e.Message);
            }
        }

        public Result PreviousSlide()
        {
            try
            {
                PresentationControl.GoToPreviousSlide();
                return new Result("Returned one slide");
            }
            catch (Exception e)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return new Result(e.Message);
            }
        }

        public Result GoToSlideNumber(string number)
        {
            try
            {
                int slideNumber = Int32.Parse(number);
                PresentationControl.GoToSlideNumber(slideNumber);
                return new Result("Went to slide number " + number);
            }
            catch (Exception e)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return new Result(e.Message);
            }
        }

        public Result ClosePresentation()
        {
            try
            {
                PresentationControl.ClosePresentation();
                return new Result("Presentation was closed");
            }
            catch(Exception e)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return new Result(e.Message);
            }
        }

        public Result UploadFile(string fileName, Stream fileContents)
        {
            if(fileManager.FileExists(fileName))
            {
                fileManager.DeleteFile(fileName);   
            }
            fileManager.CreateFile(fileName, fileContents);

            return new Result("File was uploaded to server");
        }

        public Stream GetFile(string fileName)
        {
            Stream fileStream;
            try
            {
                fileStream = new FileStream(fileName, FileMode.Open);
                return fileStream;
            }
            catch (Exception e)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                byte[] byteArray = Encoding.ASCII.GetBytes(e.Message);
                MemoryStream stream = new MemoryStream(byteArray);
                return stream;
            }
        }
        
        public Result OpenImage(string fileName)
        {
            fileName = (fileName == null || fileName == String.Empty) ? @"C:\Users\lucioc\Desktop\class_share\ClassService\Image_Pan_and_Zoom\ponei.jpg" : fileName;

            //Run image output window
            ImageForm = new ImageFormControl(fileName);
            ImageForm.RunFormInNewThread();

            //Initialize Kinect windows for gesture and speech recognition
            KinectWindow.RunWindowInNewThread();

            return new Result("Image Opened");
        }

        public Result CloseCurrentImage()
        {
            ImageForm.StopThread();

            KinectWindow.StopThread();

            return new Result("Image Closed");
        }
    }
}

