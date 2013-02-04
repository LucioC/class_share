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
    }

    [DataContract(Namespace="http://yournamespace.com")]
    public class MyContract
    {
        [DataMember(Order=1)]
        public string first { get; set;}

        [DataMember(Order=2)]
        public string last { get; set;}
    }
}
