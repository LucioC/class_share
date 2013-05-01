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
            KinectWindow.MessageSent += this.MessageReceived;
        }

        static Service()
        {        
            KinectWindow = new KinectWindowControl();
            ImageForm = new ImageFormControl();
            PresentationControl = new PowerPointControl();
            mainWindow = new KinectMainWindowControl();
            mainWindow.StartThread();
        }

        public void MessageReceived(string message)
        {
            if (message == "closeimage")
            {
                CloseCurrentImage();
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
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return new Result("wrong command argument option");
            }

            return new Result("Command Executed");
        }
        
        public Result StartPresentation(File file)
        {
            try
            {
                String localPath = fileManager.GetFilePath(file.FileName);

                //Open power point presentation
                PresentationControl.PreparePresentation(localPath);
                PresentationControl.StartPresentation();

                //Initialize Kinect windows for gesture and speech recognition
                KinectWindow.setMode(PRESENTATION_MODE.POWERPOINT);
                KinectWindow.StartThread();

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
                KinectWindow.StopThread();
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
        
        public Result OpenImage(File fileName)
        {
            try
            {
                Output.WriteToDebugOrConsole("open image function");

                fileName.FileName = fileManager.GetFilePath(fileName.FileName);

                if (ImageForm.IsThreadRunning())
                {
                    ImageForm.StopThread();
                }
                //Run image output window
                ImageForm.SetFilePath(fileName.FileName);
                ImageForm.StartThread();

                //Initialize Kinect windows for gesture and speech recognition
                KinectWindow.setMode(PRESENTATION_MODE.IMAGE);
                KinectWindow.StartThread();

                return new Result("Image Opened");
            }
            catch (Exception e)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                Output.WriteToDebugOrConsole(e.Message);
                return new Result(e.Message);
            }
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

