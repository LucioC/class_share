using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using ServiceCore.Utils;

namespace ClassService
{
    public class ServiceFileManager : ClassService.IServiceFileManager
    {
        private static String filesFolder = "files";
        private static String currentPresentationFolder = "presentation";

        private FileManager fileManager;

        public ServiceFileManager()
        {
            fileManager = new FileManager();

            FilesPath = Directory.GetCurrentDirectory() + "\\" + filesFolder + "\\";

            CurrentPresentationFolder = Directory.GetCurrentDirectory() + "\\" + currentPresentationFolder + "\\";

            System.IO.Directory.CreateDirectory(CurrentPresentationFolder);
            System.IO.Directory.CreateDirectory(FilesPath);
        }

        public List<String> GetFiles(string type)
        {
            List<String> files = new List<string>();
            if (type == null || type == String.Empty)
            {
                files.AddRange(fileManager.GetFileList(FilesPath, null));
            }
            else if (type == "image")
            {
                files.AddRange(fileManager.GetFileList(FilesPath, "*.jpg"));
                files.AddRange(fileManager.GetFileList(FilesPath, "*.png"));
            }
            else if (type == "presentation")
            {
                files.AddRange(fileManager.GetFileList(FilesPath, "*.ppt"));
            }

            return fileManager.GetFileNamesFromPaths(files);
        }

        public String FilesPath { get; protected set; }

        public String CurrentPresentationFolder { get; protected set; }

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

        public String GetPresentationSlideImageFilePath(String slideNumber)
        {
            String localPath = CurrentPresentationFolder + slideNumber + ".png";
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