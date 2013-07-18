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
using ImageZoom;
using KinectPowerPointControl;
using ServiceCore;
using ServiceCore.Utils;

namespace ClassService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Service : IService
    {
        public static IPowerPointControl PresentationControl;
        public static IImageService ImageForm;
        public static IKinectService KinectWindow;
        public static IServiceFileManager fileManager;
        private static KinectMainWindowControl mainWindow;
        private static ServiceUrlManager urlManager;

        public Service()
        {
            fileManager = new ServiceFileManager();
            mainWindow.FilesFolder = fileManager.FilesPath;
            mainWindow.StartThread();
            KinectWindow.MessageSent += this.MessageReceived;
            mainWindow.MessageSent += this.ReceiveCommand;
            urlManager = new ServiceUrlManager();
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
                    PreparePresentation(filename);
                    StartPresentation();
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

        #region presentation

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
                case PresentationAction.GOTOSLIDE: GoToSlideNumber(action.Arg);
                    break;
                default:
                    if (WebOperationContext.Current != null)
                        WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return new Result("wrong command argument option");
            }

            return new Result("Command Executed");
        }

        public Result StartPresentation()
        {
            try
            {                
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

        public Result PreparePresentation(string filename)
        {
            try
            {                
                String localPath = fileManager.GetFilePath(filename);

                //Open power point presentation
                PresentationControl.PreparePresentation(localPath);

                PresentationControl.SaveSlidesAsPNG(fileManager.CurrentPresentationFolder);

                return new Result("Presentation has been prepared");
            }
            catch (Exception e)
            {
                if (WebOperationContext.Current != null)
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return new Result("Something went wrong. Verify if the file name is corrected");
            }
        }

        public Result PreparePresentation(File fileName)
        {
            return this.PreparePresentation(fileName.FileName);
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

        #endregion

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

        public ListOfFiles GetFiles(String type)
        {
            ListOfFiles files = new ListOfFiles();
            List<String> fileNames = fileManager.GetFiles(type);

            files.Names = fileNames;
            return files;
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
                case ImageAction.ZOOM: ImageForm.SendCommand(action.Command + ":" + action.Param);
                    break;
                case ImageAction.ROTATION: ImageForm.SendCommand(action.Command + ":" + action.Param);
                    break;
                case ImageAction.MOVE: ImageForm.SendCommand(action.Command + ":" + action.Param);
                    break;
                case ImageAction.ZOOMIN: ImageForm.SendCommand(action.Command);
                    break;
                case ImageAction.ZOOMOUT: ImageForm.SendCommand(action.Command);
                    break;
                case ImageAction.ROTATERIGHT: ImageForm.SendCommand(action.Command);
                    break;
                case ImageAction.ROTATELEFT: ImageForm.SendCommand(action.Command);
                    break;
                case ImageAction.MOVERIGHT: ImageForm.SendCommand(action.Command);
                    break;
                case ImageAction.MOVELEFT: ImageForm.SendCommand(action.Command);
                    break;
                case ImageAction.MOVEUP: ImageForm.SendCommand(action.Command);
                    break;
                case ImageAction.MOVEDOWN: ImageForm.SendCommand(action.Command);
                    break;
                case ImageAction.CLOSE: CloseCurrentImage();
                    break;
                default:
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return new Result("wrong command argument option");
            }

            return new Result("Command Executed");
        }


        public Result NewEvent(ModalityEvent modalityEvent)
        {
            throw new NotImplementedException();
        }

        public ListOfImages ReturnListOfPresentationImages()
        {
            ListOfImages images = new ListOfImages();
            images.Names = new List<string>();

            string serviceUrl = urlManager.CurrentPresentationUrl();
            List<String> imageNames = urlManager.NameOfImages(PresentationControl);

            foreach(String imageName in imageNames)
            {
                images.Names.Add(imageName);
            }
            
            return images;
        }


        public Stream GetCurrentImage()
        {
            string fileName;
            Stream fileStream;
            try
            {
                fileName = ImageForm.GetImageFilePath();
                fileStream = new FileStream(fileName, FileMode.Open);

                if (WebOperationContext.Current != null)
                    WebOperationContext.Current.OutgoingResponse.ContentType = "image/png";

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

        public Stream ReturnPresentationSlideAsImage(String slideNumber)
        {
            int slide = Int32.Parse(slideNumber);

            string fileName;
            Stream fileStream;
            try
            {
                fileName = fileManager.GetPresentationSlideImageFilePath(slideNumber);
                fileStream = new FileStream(fileName, FileMode.Open);

                if (WebOperationContext.Current != null)
                    WebOperationContext.Current.OutgoingResponse.ContentType = "image/png";

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

        public PresentationInfo GetPresentationInfo()
        {
            PresentationInfo info = new PresentationInfo();

            if(PresentationControl.IsActive()) info.SlidesNumber = PresentationControl.TotalSlides();

            return info;
        }
    }
}

