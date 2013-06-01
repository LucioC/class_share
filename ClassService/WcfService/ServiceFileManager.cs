using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using ServiceCore.Utils;

namespace ClassService
{
    public class ServiceFileManager
    {
        public static String filesFolder = "files";

        private FileManager fileManager;

        public ServiceFileManager()
        {
            fileManager = new FileManager();

            FilesPath = Directory.GetCurrentDirectory() + "\\" + filesFolder + "\\";
        }

        public String FilesPath { get; protected set; }

        public String GetFilePath(String fileName)
        {
            //Get current directory and look for file
            
            String localPath = FilesPath;
            localPath = localPath + fileName;

            if (!fileManager.FileExists(localPath))
            {
                throw new FileNotFoundException("File " + fileName + " doesn't exist at server path " + FilesPath);
            }

            return localPath;
        }

        public void SaveNewFile(string fileName, Stream fileContents)
        {
            fileName = FilesPath + fileName;

            if (fileManager.FileExists(fileName))
            {
                fileManager.DeleteFile(fileName);
            }
            fileManager.CreateFile(fileName, fileContents);
        }

    }
}