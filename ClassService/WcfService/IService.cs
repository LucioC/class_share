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
        [WebInvoke(UriTemplate = "/files/{fileName}",
            Method = "PUT",
            ResponseFormat = WebMessageFormat.Json)]
        Result UploadFile(string fileName, Stream fileContents);

        [OperationContract]
        [WebInvoke(UriTemplate = "/files/{fileName}",
            Method = "GET")]
        Stream GetFile(String fileName);

        [OperationContract(Name = "StartPresentation")]
        [WebInvoke(UriTemplate = "/presentation",
            Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Result StartPresentation(File fileName);

        [OperationContract]
        [WebInvoke(UriTemplate = "/presentation/action",
            Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Result PresentationCommand(PresentationAction action);

        [OperationContract]
        [WebInvoke(UriTemplate = "/image",
            Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Result OpenImage(string fileName);
        
        [OperationContract]
        [WebInvoke(UriTemplate = "/image/action",
            Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Result ImageCommand(ImageAction action);
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
        public const String CLOSE = "close";
        
        [DataMember(Order = 1, Name = "command")]
        public string Command { get; set; }
    }

    [DataContract(Namespace = "http://yournamespace.com")]
    public class ImageAction
    {
        public ImageAction(String command)
        {
            this.Command = command;
        }

        public const String ZOOMIN = "zoomin";
        public const String ZOOMOUT = "zoomout";
        public const String ROTATERIGHT = "rotateright";
        public const String ROTATELEFT = "rotateleft";
        public const String MOVERIGHT = "moveright";
        public const String MOVELEFT = "moveleft";
        public const String CLOSE = "close";

        [DataMember(Order = 1, Name = "command")]
        public string Command { get; set; }
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
