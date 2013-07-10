using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceCore;

namespace ClassService
{
    public class ServiceUrlManager
    {
        WCFUtils wcfUtils;

        string presentationPath = "/presentation";

        public ServiceUrlManager()
        {
            wcfUtils = new WCFUtils();
        }

        public string CurrentPresentationUrl()
        {
            return wcfUtils.GetCurrentServiceURL() + presentationPath;
        }

        public List<String> NameOfImages(IPowerPointControl presentation)
        {
            if (!presentation.IsActive()) return new List<String>();

            List<String> imageNames = new List<String>();
            int slidesTotal = presentation.TotalSlides();

            for (int i = 1; i <= slidesTotal; i++)
            {
                imageNames.Add(i.ToString());
            }

            return imageNames;
        }
    }
}