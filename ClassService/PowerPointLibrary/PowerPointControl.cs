using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using System.Runtime.InteropServices;
using System.IO;

namespace PowerPointPresentation
{
    //TODO: not thread safe yet
    //TODO: remove write to console function? Maybe refactor it to a class that can allow to write or not depending on initialization.
    public class PowerPointControl
    {
        private Application application;
        private Presentation presentation;

        public PowerPointControl()
        {
        }

        public void PreparePresentation(String fileName)
        {
            //Get current directory path and put the disered filename on it
            String localPath = Directory.GetCurrentDirectory() + "\\";
            localPath = localPath + fileName;
            application = new Application();
            presentation = application.Presentations.Open2007(localPath, Microsoft.Office.Core.MsoTriState.msoTrue, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue);
        }

        public void StartPresentation()
        {
            SlideShowSettings sst = presentation.SlideShowSettings;
            sst.ShowType = Microsoft.Office.Interop.PowerPoint.PpSlideShowType.ppShowTypeSpeaker;
            sst.Run();
        }

        public int CurrentSlide()
        {
            return presentation.SlideShowWindow.View.CurrentShowPosition;
        }

        public void GoToFirstSlide()
        {
            presentation.SlideShowWindow.View.First();
        }

        public void GoToLastSlide()
        {
            presentation.SlideShowWindow.View.Last();
        }

        public void GoToSlideNumber(int slideNumber)
        {
            presentation.SlideShowWindow.View.GotoSlide(slideNumber);
        }

        public void GoToNextSlide()
        {
            presentation.SlideShowWindow.View.Next();
        }

        public void GoToPreviousSlide()
        {
            presentation.SlideShowWindow.View.Previous();
        }

        public int TotalSlides()
        {
            return presentation.Slides.Count;
        }

        public void ClosePresentation()
        {
            presentation.Close();
        }
    }
}
