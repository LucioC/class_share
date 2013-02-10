using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ClassService;
using System.ServiceModel.Description;
using System.ServiceModel.Web;

namespace ServiceHostConsole
{
    class Program
    {
        private static System.Threading.AutoResetEvent stopFlag = new System.Threading.AutoResetEvent(false);
 
        public static void Main()
        {
            WebServiceHost host = new WebServiceHost(typeof(Service), new Uri("http://10.1.1.7:8880"));
            ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IService), new WebHttpBinding(), "");
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
