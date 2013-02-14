using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ClassService;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Xml;

namespace ServiceHostConsole
{
    class Program
    {
        private static System.Threading.AutoResetEvent stopFlag = new System.Threading.AutoResetEvent(false);
 
        public static void Main()
        {
            WebServiceHost host = new WebServiceHost(typeof(Service), new Uri("http://10.1.1.7:8880"));
            WebHttpBinding webHttpBinding = new WebHttpBinding();
            webHttpBinding.MaxReceivedMessageSize = 2147483647;
            webHttpBinding.MaxBufferPoolSize = 2147483647;

            webHttpBinding.ReaderQuotas.MaxArrayLength = 2147483647;
            webHttpBinding.ReaderQuotas.MaxBytesPerRead = 2147483647;
            webHttpBinding.ReaderQuotas.MaxDepth = 2147483647;
            webHttpBinding.ReaderQuotas.MaxNameTableCharCount = 2147483647;
            webHttpBinding.ReaderQuotas.MaxStringContentLength = 2147483647;

            ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IService), webHttpBinding, "");
            ServiceDebugBehavior stp = host.Description.Behaviors.Find<ServiceDebugBehavior>();
            stp.HttpHelpPageEnabled = false;
            host.Open();
            Console.WriteLine("Service is up and running");
            Console.WriteLine("Press enter to quit ");
            Console.ReadLine();
            host.Close();
        }
    
    }
}
