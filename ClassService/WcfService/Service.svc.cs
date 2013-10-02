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
using System.ServiceModel.Channels;

namespace ClassService
{
    /// <summary>
    /// This class implements the functionalities of the WCF class service.
    /// It uses collaborators to control the different parts of the system. 
    /// For purposes of the WCF implementation, they are static members and should be initialized before the use of the Service.
    /// Since WCF does not provide an easy DI, the initialization should be done outside and before the use of this class setting the static instances. 
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Service : IService
    {
        public static IPowerPointControl PresentationControl;
        public static IImageService ImageForm;
        public static IKinectService KinectWindow;
        public static IServiceFileManager fileManager;
        public static IKinectMainWindowControl mainWindow;
        public static ServiceUrlManager urlManager;

        public static String CurrentSlidePresentation = "";
        public static String CurrentImagePresentation = "";
        
        private WCFUtils wcfUtils;

        public static ListenerList slidesListeners;
        public static ListenerList imageListeners;

        public Service()
        {
            wcfUtils = new WCFUtils();
            slidesListeners = new ListenerList();
            imageListeners = new ListenerList();
        }

        public void prepareService()
        {
            mainWindow.FilesFolder = fileManager.FilesPath;
            mainWindow.StartThread();
            KinectWindow.Executor = this.ExecutePresentationCommand;
            mainWindow.Executor = this.MainWindowCommand;
            ImageForm.ImageUpdate += this.ImageUpdated;
        }

        /// <summary>
        /// Register the user IP in a list so then the user will receive updates from the server.
        /// </summary>
        /// <returns></returns>
        public Result AddListener(PresentationMode presentationMode)
        {
            String userIp = wcfUtils.GetCurrentRequestorIP();
            ClientUpdater newClientListener = new ClientUpdater(new RESTJsonClient());
            newClientListener.ClientIP = userIp;

            if (presentationMode.Mode == PresentationMode.Slides)
            {
                if (slidesListeners.Add(newClientListener))
                {
                    return new Result("New listener added");
                }
                else
                {
                    return new Result("This Listener is already registered");
                }
            }
            else if (presentationMode.Mode == PresentationMode.Image)
            {
                if (imageListeners.Add(newClientListener))
                {
                    return new Result("New listener added");
                }
                else
                {
                    return new Result("This Listener is already registered");
                }
            }

            return new Result("Wrong mode option");
        }

        private void ImageUpdated(ImageState imageState)
        {
            if (!imageState.Active)
            {
                this.CloseCurrentImage();
            }
            WarnImagePresentationListeners();
        }

        private void WarnSlidePresentationListeners()
        {
            String responsable = GetCurrentEventTriggerResponsable();

            slidesListeners.WarnListenersExcept(responsable);
        }

        private void WarnImagePresentationListeners()
        {
            String responsable = GetCurrentEventTriggerResponsable();

            imageListeners.WarnListenersExcept(responsable);
        }

        private String GetCurrentEventTriggerResponsable()
        {
            String responsable = "internal";

            if (WebOperationContext.Current != null)
            {
                responsable = wcfUtils.GetCurrentRequestorIP();
            }
            return responsable;
        }

        /// <summary>
        /// Remove the user from the list of listeners so he will not receive event updates from the server.
        /// </summary>
        /// <returns></returns>
        public Result RemoveListener()
        {
            String userIp = wcfUtils.GetCurrentRequestorIP();
            if ( slidesListeners.Remove(userIp) || imageListeners.Remove(userIp) )
            {
                return new Result("Listener was removed");
            }
            else
            {
                return new Result("No listener with this IP exist");
            }
        }

        public ImageInfo GetImageInfo()
        {
            ImageInfo imageInfo = new ImageInfo();

            imageInfo.FileName = CurrentImagePresentation;
            if (ImageForm.IsThreadRunning())
            {
                ImageState imageState = ImageForm.ImageState;
                imageInfo.Bottom = imageState.Bottom;
                imageInfo.Height = imageState.ScreenHeight;
                imageInfo.Left = imageState.Left;
                imageInfo.Right = imageState.Right;
                imageInfo.Rotation = imageState.Angle;
                imageInfo.Top = imageState.Top;
                imageInfo.Width = imageState.ScreenWidth;
            }

            Log("Get Image Info");
            return imageInfo;
        }

        private void MainWindowCommand(string message, string param)
        {
            string command = message;

            if (command == "open")
            {
                string filename = param;

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

        public void ExecutePresentationCommand(string command, string param)
        {
            float value = 0;
            if (param != null && param != "") value = float.Parse(param);
            switch (command)
            {
                case ServiceCommands.CLOSE_IMAGE:
                    CloseCurrentImage();
                    WarnImagePresentationListeners();
                    break;

                case ServiceCommands.CLOSE_PRESENTATION:
                    ClosePresentation();
                    break;

                case ServiceCommands.NEXT_SLIDE:
                    NextSlide();
                    break;

                case ServiceCommands.PREVIOUS_SLIDE:
                    PreviousSlide();
                    break;

                case ServiceCommands.IMAGE_MOVE_X:
                case ServiceCommands.IMAGE_MOVE_Y:
                case ServiceCommands.IMAGE_ROTATE:
                case ServiceCommands.IMAGE_ZOOM:
                    ImageAction action = new ImageAction(command);
                    action.Param = param;
                    ImageCommand(action);
                    break;
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

            //WarnSlidePresentationListeners();
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

                mainWindow.PauseEvents();

                Log("Start Presentation Command");
                return new Result("Presentation has been started");
            }
            catch (Exception e)
            {
                if (WebOperationContext.Current != null)
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return new Result("Something went wrong. Verify if the file name is corrected");
            }
        }

        public void Log(String message)
        {
            Output.Debug("Service", message);
        }

        private void setSlidePresentationName(String fileName)
        {
            CurrentSlidePresentation = fileName;
        }

        private void setImagePresentationName(String fileName)
        {
            CurrentImagePresentation = fileName;
        }

        public Result PreparePresentation(string filename)
        {
            try
            {                
                String localPath = fileManager.GetFilePath(filename);

                //Open power point presentation
                PresentationControl.PreparePresentation(localPath);

                PresentationControl.SaveSlidesAsPNG(fileManager.CurrentPresentationFolder);

                setSlidePresentationName(filename);

                Log("Prepare Presentation");
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
                WarnSlidePresentationListeners();

                Log("Next Slide");
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
                WarnSlidePresentationListeners();

                Log("Previous slide");
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
                WarnSlidePresentationListeners();

                Log("Go to slide");
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
                if (PresentationControl.IsActive())
                {
                    KinectWindow.StopThread();
                    PresentationControl.ClosePresentation();
                    mainWindow.RestartEvents();
                    setSlidePresentationName("");
                    WarnSlidePresentationListeners();

                    Log("Close Presentation");
                    return new Result("Presentation was closed");
                }
                else
                {
                    return new Result("Presentation is not active");
                }
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

                Log("Get File");
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

            Log("Get Files");

            files.Names = fileNames;
            return files;
        }

        public Result OpenImage(string filename)
        {
            try
            {
                Output.WriteToDebugOrConsole("open image function");

                String localPath = fileManager.GetFilePath(filename);

                setImagePresentationName(filename);

                if (ImageForm.IsThreadRunning())
                {
                    ImageForm.StopThread();
                }

                //Run image output window
                ImageForm.SetFilePath(localPath);
                ImageForm.StartThread();

                //Initialize Kinect windows for gesture and speech recognition
                KinectWindow.setMode(PRESENTATION_MODE.IMAGE);
                KinectWindow.StartThread();
                mainWindow.PauseEvents();

                Log("Open Image");

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
            if (ImageForm.IsThreadRunning())
            {
                ImageForm.StopThread();

                KinectWindow.StopThread();
                mainWindow.RestartEvents();

                setImagePresentationName("");

                Log("Close Image");

                return new Result("Image Closed");
            }
            return new Result("Image is Already Closed");
        }

        public Result ImageCommand(ImageAction action)
        {
            switch (action.Command)
            {
                case ServiceCommands.IMAGE_MOVE_X:
                case ServiceCommands.IMAGE_MOVE_Y:
                case ServiceCommands.IMAGE_ROTATE:
                case ServiceCommands.IMAGE_SET_VISIBLE_PART:
                case ServiceCommands.IMAGE_ZOOM:
                    if (ImageForm.Executor != null)
                        ImageForm.Executor.BeginInvoke(action.Command, action.Param, null, null);
                    break;
                case ServiceCommands.CLOSE_IMAGE: CloseCurrentImage();
                    break;
                default:
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return new Result("wrong command argument option");
            }

            Log("Image Command: " + action.Command);

            //WarnImagePresentationListeners();
            return new Result("Command Executed");
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

                Log("Get Current Image");

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

                Log("Get slide as image " + slideNumber);

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

            if (PresentationControl.IsActive())
            {
                info.SlidesNumber = PresentationControl.TotalSlides();
                info.CurrentSlide = PresentationControl.CurrentSlide();
                info.FileName = CurrentSlidePresentation;
            }
            else
            {
                info.SlidesNumber = 0;
                info.CurrentSlide = 0;
                info.FileName = "";
            }

            Log("Get presentation info");

            return info;
        }
    }
}

