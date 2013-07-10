using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Web;

namespace ClassService
{
    public class WCFUtils
    {
        public String GetCurrentServiceURL()
        {
            return WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri.ToString();
        }
    }
}