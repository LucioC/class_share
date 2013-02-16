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
        Uri address = new Uri("http://localhost:2475/Service1.svc/");

        Uri openAddress = new Uri("http://localhost:8880/presentation");
        Uri nextAddress = new Uri("http://localhost:8880/presentation/action");
        Uri previousAddress = new Uri("http://localhost:8880/presentation/action");
        // Uri goto5Address = new Uri("http://localhost:8880/presentation/slide?number=5");
        Uri closeAddress = new Uri("http://localhost:8880/presentation/action");
        Uri sendFileAddress = new Uri("http://localhost:8880/files/filename");

        String fileName = "C:/Users/lucioc/Dropbox/Public/Mestrado/Dissertacao/PEP/PEP_posM.pptx";

        static void Main(string[] args)
        {
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);

            Program p = new Program();

            //p.OpenNextPreviousClosePresentation();
            p.PostFile();

            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
        }

        public void PostFile()
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Open);
            Console.Write("file stream presente " + fileStream != null);

            postFile(sendFileAddress, fileStream, "application/vnd.openxmlformats-officedocument.presentationml.presentation", (int)fileStream.Length);
            fileStream.Close();
        }

        public void OpenNextPreviousClosePresentation()
        {
            put(openAddress, "{\"fileName\":\"" + fileName + "\"}", "application/json");

            put(nextAddress, "{\"command\":\"next\"}", "application/json");
            put(nextAddress, "{\"command\":\"next\"}", "application/json");
            put(nextAddress, "{\"command\":\"next\"}", "application/json");
            put(nextAddress, "{\"command\":\"next\"}", "application/json");
            put(nextAddress, "{\"command\":\"next\"}", "application/json");

            System.Threading.Thread.Sleep(1000);
            put(nextAddress, "{\"command\":\"previous\"}", "application/json");
            put(nextAddress, "{\"command\":\"previous\"}", "application/json");

            System.Threading.Thread.Sleep(1000);
            put(nextAddress, "{\"command\":\"previous\"}", "application/json");
            put(nextAddress, "{\"command\":\"previous\"}", "application/json");

            System.Threading.Thread.Sleep(2000);
            put(nextAddress, "{\"command\":\"close\"}", "application/json");
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

        public void put(Uri address, String content, String contentType)
        {
            // Create the web request  
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

            // Set type to POST  
            request.Method = "PUT";
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

        byte[] StreamToByteArray(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public void postFile(Uri address, Stream stream, String contentType, int length = 64)
        {
            // Create the web request  
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

            // Set type to POST  
            request.Method = "POST";
            request.ContentType = contentType;

            // Create a byte array of the data we want to send  
            byte[] byteData = StreamToByteArray(stream);

            // Set the content length in the request headers  
            request.ContentLength = length;

            // Write data  
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, length);
            }

            WebResponse r = null;
            try
            {
                r = request.GetResponse();
            }
            catch (WebException e)
            {
                writeToConsoleAndOutput(e.Message);
                writeToConsoleAndOutput(e.Response.GetResponseStream().ToString());
            }
            // Get response  
            using (HttpWebResponse response = r as HttpWebResponse)
            {
                // Get the response stream  
                StreamReader reader = new StreamReader(response.GetResponseStream());

                // Console application output  
                Console.WriteLine(reader.ReadToEnd());
            }
        }


        public void writeToConsoleAndOutput(String message)
        {
            System.Diagnostics.Debug.Write("\n\n" + message + "\n\n");
            Console.WriteLine("\n\n" + message + "\n\n");
        }

    }
}