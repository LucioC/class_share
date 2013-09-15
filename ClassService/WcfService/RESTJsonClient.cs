using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.IO;

namespace ClassService
{
    public class RESTJsonClient
    {
        public void Put(Uri address, String content, String contentType)
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
    }
}