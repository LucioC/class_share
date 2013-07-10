using System;
using System.Collections.Generic;
namespace ClassService
{
    public interface IServiceFileManager
    {
        string CurrentPresentationFolder { get; }
        string FilesPath { get; }
        string GetFilePath(string fileName);
        string GetPresentationSlideImageFilePath(string slideNumber);
        void SaveNewFile(string fileName, System.IO.Stream fileContents);
        List<String> GetFiles(string type);
    }
}
