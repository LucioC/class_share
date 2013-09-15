using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Web;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ClassService
{
    public class WCFUtils
    {
        public String GetCurrentServiceURL()
        {
            return WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri.ToString();
        }

        public String GetCurrentRequestorIP()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint =
                prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            return endpoint.Address;
        }
    }
}