using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class Service1 : IService1
    {
        public static String last = "Lucio";

        public MyContract AddParameter(MyContract name)
        {
            last = name.first;            
            return name;
        }
        public String Add()
        {
            return last;
        }
    }
}

