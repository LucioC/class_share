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

            for (int i = 0; i < 5; i++ )
            {
                string input = Console.ReadLine();

                Program p = new Program();
                p.get(address);

                p.post(address, "{\"first\":\"teste\",\"last\":\"te\"}", "application/json");
            }
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