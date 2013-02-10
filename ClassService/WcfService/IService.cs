using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;

namespace ClassService
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract(Name = "AddParameter")]
        [WebInvoke(
            UriTemplate = "/", 
            Method = "POST", 
            ResponseFormat = WebMessageFormat.Json,            
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        MyContract AddParameter(MyContract name);

        [OperationContract(Name = "Add")]
        [WebGet(UriTemplate = "/", 
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.Bare)]
        String Add();
        
        [OperationContract(Name = "StartPresentation")]
        [WebInvoke(UriTemplate = "/presentation/open?fileName={fileName}",
            Method = "GET", 
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Result StartPresentation(String fileName);

        [OperationContract(Name = "NextSlide")]
        [WebInvoke(UriTemplate = "/presentation/next",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Result NextSlide();

        [OperationContract(Name = "PreviousSlide")]
        [WebInvoke(UriTemplate = "/presentation/previous",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Result PreviousSlide();
        
        [OperationContract(Name = "GoToSlideNumber")]
        [WebInvoke(UriTemplate = "/presentation/slide?number={number}",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Result GoToSlideNumber(String number);

        [OperationContract(Name = "ClosePresentation")]
        [WebInvoke(UriTemplate = "/presentation/close",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Result ClosePresentation();
    }

    [DataContract(Namespace="http://yournamespace.com")]
    public class MyContract
    {
        [DataMember(Order=1)]
        public string first { get; set;}

        [DataMember(Order=2)]
        public string last { get; set;}
    }

    [DataContract(Namespace = "http://yournamespace.com")]
    public class Result
    {
        public Result(String message)
        {
            this.Message = message;
        }

        [DataMember(Order = 1)]
        public string Message { get; set; }
    }
}
