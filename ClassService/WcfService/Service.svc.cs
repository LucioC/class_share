using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.Office.Interop.PowerPoint;
using PowerPointPresentation;
using System.IO;

namespace ClassService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Service : IService
    {
        private static PowerPointControl presentationControl;

        public Service()
        {
            presentationControl = new PowerPointControl();
        }

        public Service(PowerPointControl powerPointControl)
        {
            presentationControl = powerPointControl;
        }
        
        public Result StartPresentation(String fileName)
        {
            if (presentationControl.PreparePresentation(fileName))
            {
                presentationControl.StartPresentation();
                return new Result("Presentation has been started");
            }
            else
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return new Result("Something went wrong. Verify if the file name is corrected");
            }
        }

        public Result NextSlide()
        {
            try
            {
                presentationControl.GoToNextSlide();
                return new Result("Advanced one slide");
            }
            catch (Exception e)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return new Result(e.Message);
            }
        }

        public Result PreviousSlide()
        {
            try
            {
                presentationControl.GoToPreviousSlide();
                return new Result("Returned one slide");
            }
            catch (Exception e)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return new Result(e.Message);
            }
        }


        public Result GoToSlideNumber(string number)
        {
            try
            {
                int slideNumber = Int32.Parse(number);
                presentationControl.GoToSlideNumber(slideNumber);
                return new Result("Went to slide number " + number);
            }
            catch (Exception e)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return new Result(e.Message);
            }
        }


        public Result ClosePresentation()
        {
            try
            {
                presentationControl.ClosePresentation();
                return new Result("Presentation was closed");
            }
            catch(Exception e)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return new Result(e.Message);
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

        public String UploadPhoto(string fileName, Stream fileContents)
        {
            byte[] buffer = StreamToByteArray(fileContents);
            
            if (!System.IO.File.Exists(fileName))
            {
                using (System.IO.FileStream fs = System.IO.File.Create(fileName))
                {
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Close();
                }                
            }
            Console.WriteLine("Uploaded file {0} with {1} bytes", fileName, buffer.Length);

            return "ok";
        }
        
        Stream GetFile(string fileName)
        {
            Stream fileStream;
            try
            {
                fileStream = new FileStream(fileName, FileMode.Open);
                return fileStream;
            }
            catch (Exception e)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                byte[] byteArray = Encoding.ASCII.GetBytes(e.Message);
                MemoryStream stream = new MemoryStream(byteArray);
                return stream;
            }
        }
    }
}

