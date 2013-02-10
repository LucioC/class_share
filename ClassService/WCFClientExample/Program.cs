using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Runtime.Serialization;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Web;

namespace ConsoleTestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri address = new Uri("http://localhost:2475/Service1.svc/");

            Uri openAddress = new Uri("http://localhost:8880/presentation/open?fileName=C:/Users/lucioc/Dropbox/Public/Mestrado/Dissertacao/PEP/PEP_posM.pptx");
            Uri nextAddress = new Uri("http://localhost:8880/presentation/next");
            Uri previousAddress = new Uri("http://localhost:8880/presentation/previous");
            Uri goto5Address = new Uri("http://localhost:8880/presentation/slide?number=5");
            Uri closeAddress = new Uri("http://localhost:8880/presentation/close");

            
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);

            Program p = new Program();
            /*for (int i = 0; i < 5; i++ )
            {
                string input = Console.ReadLine();

                Program p = new Program();
                p.get(address);

                p.post(address, "{\"first\":\"teste\",\"last\":\"te\"}", "application/json");
            }*/

            p.get(openAddress);

            p.get(nextAddress);
            p.get(nextAddress);
            p.get(nextAddress);
            p.get(nextAddress);
            p.get(nextAddress);

            System.Threading.Thread.Sleep(1000);
            p.get(previousAddress);
            p.get(previousAddress);

            System.Threading.Thread.Sleep(1000);
            p.get(previousAddress);
            p.get(previousAddress);

            System.Threading.Thread.Sleep(900);
            p.get(goto5Address);
            
            System.Threading.Thread.Sleep(2000);
            p.get(closeAddress);

            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
        }

        public void get(Uri address)
        {
            // Create the web request  
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            
            // Get response  
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                // Get the response stream  
                StreamReader reader = new StreamReader(response.GetResponseStream());

                // Console application output
                Console.WriteLine(reader.ReadToEnd());
            }  
        }

        public void post(Uri address, String content, String contentType)
        {   
            // Create the web request  
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

            // Set type to POST  
            request.Method = "POST";
            request.ContentType = contentType;
                        
            // Create a byte array of the data we want to send  
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(content);

            // Set the content length in the request headers  
            request.ContentLength = byteData.Length;

            // Write data  
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            
            // Get response  
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                // Get the response stream  
                StreamReader reader = new StreamReader(response.GetResponseStream());

                // Console application output  
                Console.WriteLine(reader.ReadToEnd());
            }  
        }
    
    }
}