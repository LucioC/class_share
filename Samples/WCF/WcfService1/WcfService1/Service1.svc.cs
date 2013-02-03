using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.Office.Interop.PowerPoint;

namespace WcfService1
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class Service1 : IService1
    {
        public static String last = "Lucio";

        public MyContract AddParameter(MyContract name)
        {
            last = name.first;            
            return name;
        }
        public String Add()
        {
            return last;
        }

        public String openPresentation(String fileName)
        {
            Application application = new Application();
            Presentation presentation = application.Presentations.Open2007(fileName, Microsoft.Office.Core.MsoTriState.msoTrue, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue);
            SlideShowSettings sst = presentation.SlideShowSettings;
            sst.ShowType = Microsoft.Office.Interop.PowerPoint.PpSlideShowType.ppShowTypeSpeaker;
            sst.Run();

            return null;
        }
    }
}

