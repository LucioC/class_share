using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using System.IO;

namespace ClassService
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/listeners",
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json)]
        Result AddListener(PresentationMode mode);

        [OperationContract]
        [WebInvoke(UriTemplate = "/listeners",
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json)]
        Result RemoveListener();
        
        [OperationContract]
        [WebInvoke(UriTemplate = "/image/info",
            Method = "GET",
            ResponseFormat = WebMessageFormat.Json)]
        ImageInfo GetImageInfo();

        [OperationContract]
        [WebInvoke(UriTemplate = "/files/{fileName}",
            Method = "PUT",
            ResponseFormat = WebMessageFormat.Json)]
        Result UploadFile(string fileName, Stream fileContents);

        [OperationContract]
        [WebInvoke(UriTemplate = "/files/{fileName}",
            Method = "GET")]
        Stream GetFile(String fileName);

        [OperationContract]
        [WebInvoke(UriTemplate = "/files?type={type}",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        ListOfFiles GetFiles(String type);

        [OperationContract]
        [WebInvoke(UriTemplate = "/presentation/slides/{slideNumber}",
            Method = "GET")]
        Stream ReturnPresentationSlideAsImage(String slideNumber);

        [OperationContract]
        [WebInvoke(UriTemplate = "/presentation/info",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        PresentationInfo GetPresentationInfo();

        [OperationContract]
        [WebInvoke(UriTemplate = "/presentation",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        ListOfImages ReturnListOfPresentationImages();

        [OperationContract(Name = "StartPresentation")]
        [WebInvoke(UriTemplate = "/presentation",
            Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Result StartPresentation();

        [OperationContract(Name = "PreparePresentation")]
        [WebInvoke(UriTemplate = "/presentation/prepare",
            Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Result PreparePresentation(File fileName);

        [OperationContract]
        [WebInvoke(UriTemplate = "/presentation/action",
            Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Result PresentationCommand(PresentationAction action);

        [OperationContract]
        [WebInvoke(UriTemplate = "/image",
            Method = "GET")]
        Stream GetCurrentImage();

        [OperationContract]
        [WebInvoke(UriTemplate = "/image",
            Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Result OpenImage(File fileName);
        
        [OperationContract]
        [WebInvoke(UriTemplate = "/image/action",
            Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Result ImageCommand(ImageAction action);
    }

    [DataContract(Namespace = "http://yournamespace.com")]
    public class ListOfImages
    {
        public ListOfImages()
        {
        }

        [DataMember(Order = 1, Name = "images")]
        public List<String> Names { get; set; }
    }

    [DataContract(Namespace = "http://yournamespace.com")]
    public class ListOfFiles
    {
        public ListOfFiles()
        {
        }

        [DataMember(Order = 1, Name = "files")]
        public List<String> Names { get; set; }
    }

    [DataContract(Namespace = "http://yournamespace.com")]
    public class PresentationInfo
    {
        public PresentationInfo()
        {
        }

        [DataMember(Order = 1, Name = "slidesNumber")]
        public int SlidesNumber { get; set; }
        
        [DataMember(Order = 2, Name = "currentSlide")]
        public int CurrentSlide { get; set; }

        [DataMember(Order = 3, Name = "fileName")]
        public String FileName { get; set; }
    }

    [DataContract(Namespace = "http://yournamespace.com")]
    public class ImageInfo
    {
        public ImageInfo()
        {
        }

        [DataMember(Order = 1, Name = "left")]
        public int Left { get; set; }

        [DataMember(Order = 2, Name = "top")]
        public int Top { get; set; }

        [DataMember(Order = 3, Name = "right")]
        public int Right { get; set; }

        [DataMember(Order = 4, Name = "bottom")]
        public int Bottom { get; set; }

        [DataMember(Order = 5, Name = "height")]
        public int Height { get; set; }

        [DataMember(Order = 6, Name = "width")]
        public int Width { get; set; }

        [DataMember(Order = 7, Name = "rotation")]
        public int Rotation { get; set; }

        [DataMember(Order = 8, Name = "fileName")]
        public String FileName { get; set; }
    }
    
    [DataContract(Namespace = "http://yournamespace.com")]
    public class File
    {
        public File(String filename)
        {
            this.FileName = filename;
        }
        
        [DataMember(Order = 1, Name = "fileName")]
        public string FileName { get; set; }
    }

    [DataContract(Namespace = "http://yournamespace.com")]
    public class PresentationAction
    {
        public PresentationAction(String command)
        {
            this.Command = command;
        }

        public const String NEXT = "next";
        public const String PREVIOUS = "previous";
        public const String GOTOSLIDE = "gotoslide";
        public const String CLOSE = "close";
        
        [DataMember(Order = 1, Name = "command")]
        public string Command { get; set; }

        [DataMember(Order = 2, Name = "arg")]
        public string Arg { get; set; }
    }

    /// <summary>
    /// Represents an action to happen to the image.
    /// The 'Param' variable can have additional parameters to the command.
    /// The VIEWBOUNDS ('visiblepart') update has the following format:
    /// left:top:right:bottom:imageH:imageW:rotation
    /// </summary>
    [DataContract(Namespace = "http://yournamespace.com")]
    public class ImageAction
    {
        public ImageAction(String command)
        {
            this.Command = command;
        }

        [DataMember(Order = 1, Name = "command")]
        public string Command { get; set; }
        
        [DataMember(Order = 2, Name = "param")]
        public string Param { get; set; }
    }

    [DataContract(Namespace = "http://yournamespace.com")]
    public class ModalityEvent
    {
        [DataMember(Order = 1, Name = "eventName")]
        public String EventName { get; set; }
    }

    [DataContract(Namespace = "http://yournamespace.com")]
    public class PresentationMode
    {
        public static String Slides = "slides";
        public static String Image = "image";

        [DataMember(Order = 1, Name = "mode")]
        public String Mode { get; set; }
    }

    [DataContract(Namespace = "http://yournamespace.com")]
    public class Result
    {
        public Result(String message)
        {
            this.Message = message;
        }

        [DataMember(Order = 1, Name = "message")]
        public string Message { get; set; }
    }

}
