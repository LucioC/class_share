using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ServiceCore;

namespace CommonUtils
{
    public class FileManager
    {
        public Boolean FileExists(String fileName)
        {
            return System.IO.File.Exists(fileName);
        }

        public String ReadFile(String fileName)
        {
            Stream fileStream = new FileStream(fileName, FileMode.Open);
            StreamReader reader = new StreamReader(fileStream);
            string text = reader.ReadToEnd();
            fileStream.Close();
            return text;
        }

        public void CreateFile(String fileName, String fileContent)
        {
            byte[] fileBytes = Encoding.ASCII.GetBytes(fileContent);
            WriteToFile(fileName, fileBytes);
        }

        public void CreateFile(String fileName, Stream fileContent)
        {
            byte[] fileBytes = StreamToByteArray(fileContent);
            WriteToFile(fileName,fileBytes);
        }

        public void WriteToFile(String fileName, byte[] byteContent)
        {
            using (System.IO.FileStream fs = System.IO.File.Create(fileName))
            {
                fs.Write(byteContent, 0, byteContent.Length);
                fs.Close();
            }
        }

        public byte[] StreamToByteArray(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public void DeleteFile(String fileName)
        {
            System.IO.File.Delete(fileName);
        }
    }
}
