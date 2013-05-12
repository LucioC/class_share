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
using ServiceCore;

namespace ClassService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Service : IService
    {
        public static IPowerPointControl PresentationControl;
        public static IImageService ImageForm;
        public static IKinectService KinectWindow;
        private ServiceFileManager fileManager;
        private static KinectMainWindowControl mainWindow;

        public Service()
        {
            fileManager = new ServiceFileManager();
            mainWindow.FilesFolder = fileManager.FilesPath;
            mainWindow.StartThread();
            KinectWindow.MessageSent += this.MessageReceived;
            mainWindow.MessageSent += this.ReceiveCommand;
        }

        static Service()
        {        
            KinectWindow = new KinectWindowControl();
            ImageForm = new ImageFormControl();
            PresentationControl = new PowerPointControl();
            mainWindow = new KinectMainWindowControl();
        }

        private void ReceiveCommand(string message)
        {
            string command = message;
            int index = message.IndexOf(":");
            if (index >= 0)
            {
                command = message.Substring(0, index);
            }

            if (command == "open")
            {
                string filename = message.Substring(index+1);

                string extension = System.IO.Path.GetExtension(filename);

                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                {
                    OpenImage(filename);
                }
                else if(extension == ".pptx" || extension == ".ppt")
                {
                    StartPresentation(filename);
                }
            }
        }

        public void MessageReceived(string message)
        {
            if (message == "closeimage")
            {
                CloseCurrentImage();
            }
            else if (message == "closepresentation")
            {
                ClosePresentation();
            }
        }

        public Result PresentationCommand(PresentationAction action)
        {
            switch (action.Command)
            {
                case PresentationAction.NEXT: NextSlide();
                    break;
                case PresentationAction.PREVIOUS: PreviousSlide();
                    break;
                case PresentationAction.CLOSE: ClosePresentation();
                    break;
                default:
                    if (WebOperationContext.Current != null)
                        WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return new Result("wrong command argument option");
            }

            return new Result("Command Executed");
        }

        public Result StartPresentation(string filename)
        {
            try
            {
                String localPath = fileManager.GetFilePath(filename);

                //Open power point presentation
                PresentationControl.PreparePresentation(localPath);
                PresentationControl.StartPresentation();

                //Initialize Kinect windows for gesture and speech recognition
                KinectWindow.setMode(PRESENTATION_MODE.POWERPOINT);
                KinectWindow.StartThread();

                return new Result("Presentation has been started");
            }
            catch (Exception e)
            {
                if (WebOperationContext.Current != null)
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return new Result("Something went wrong. Verify if the file name is corrected");
            }
        }

        public Result StartPresentation(File file)
        {
            return this.StartPresentation(file.FileName);
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
                if (WebOperationContext.Current != null)
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
                if (WebOperationContext.Current != null)
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
                if (WebOperationContext.Current != null)
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return new Result(e.Message);
            }
        }

        public Result ClosePresentation()
        {
            try
            {
                KinectWindow.StopThread();
                PresentationControl.ClosePresentation();
                return new Result("Presentation was closed");
            }
            catch(Exception e)
            {
                if(WebOperationContext.Current != null)
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return new Result(e.Message);
            }
        }

        public Result UploadFile(string fileName, Stream fileContents)
        {
            Output.WriteToDebugOrConsole("Entered uploadFile call");
            fileManager.SaveNewFile(fileName, fileContents);

            return new Result("File was uploaded to server");
        }

        public Stream GetFile(string fileName)
        {
            Stream fileStream;
            try
            {
                fileName = fileManager.GetFilePath(fileName);
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

        public Result OpenImage(string filename)
        {
            try
            {
                Output.WriteToDebugOrConsole("open image function");

                filename = fileManager.GetFilePath(filename);

                if (ImageForm.IsThreadRunning())
                {
                    ImageForm.StopThread();
                }

                //Run image output window
                ImageForm.SetFilePath(filename);
                ImageForm.StartThread();

                //Initialize Kinect windows for gesture and speech recognition
                KinectWindow.setMode(PRESENTATION_MODE.IMAGE);
                KinectWindow.StartThread();

                return new Result("Image Opened");
            }
            catch (Exception e)
            {
                if (WebOperationContext.Current != null)
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                Output.WriteToDebugOrConsole(e.Message);
                return new Result(e.Message);
            }
        }

        public Result OpenImage(File fileName)
        {
            return OpenImage(fileName.FileName);
        }

        public void ImageRotateRight()
        {
            ImageForm.SendCommand("rotateright");
        }

        public void ImageRotateLeft()
        {
            ImageForm.SendCommand("rotateleft");
        }

        public void ImageZoomIn()
        {
            ImageForm.SendCommand("zoomin");
        }

        public void ImageZoomOut()
        {
            ImageForm.SendCommand("zoomout");
        }

        public void ImageMoveRight()
        {
            ImageForm.SendCommand("moveright");
        }

        public void ImageMoveUp()
        {
            ImageForm.SendCommand("moveup");
        }

        public void ImageMoveDown()
        {
            ImageForm.SendCommand("movedown");
        }

        public void ImageMoveLeft()
        {
            ImageForm.SendCommand("moveleft");
        }

        public Result CloseCurrentImage()
        {
            ImageForm.StopThread();

            KinectWindow.StopThread();

            return new Result("Image Closed");
        }

        public Result ImageCommand(ImageAction action)
        {
            switch (action.Command)
            {
                case ImageAction.ZOOMIN: ImageZoomIn();
                    break;
                case ImageAction.ZOOMOUT: ImageZoomOut();
                    break;
                case ImageAction.ROTATERIGHT: ImageRotateRight();
                    break;
                case ImageAction.ROTATELEFT: ImageRotateLeft();
                    break;
                case ImageAction.MOVERIGHT: ImageMoveRight();
                    break;
                case ImageAction.MOVELEFT: ImageMoveLeft();
                    break;
                case ImageAction.MOVEUP: ImageMoveUp();
                    break;
                case ImageAction.MOVEDOWN: ImageMoveDown();
                    break;
                case ImageAction.CLOSE: CloseCurrentImage();
                    break;
                default:
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return new Result("wrong command argument option");
            }

            return new Result("Command Executed");
        }
    }
}

