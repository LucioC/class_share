using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using System.Runtime.InteropServices;
using System.IO;
using ServiceCore;

namespace PowerPointPresentation
{
    //TODO: not thread safe yet
    //TODO: remove write to console function? Maybe refactor it to a class that can allow to write or not depending on initialization.
    public class PowerPointControl : IPowerPointControl
    {
        private Application application;
        private Presentation presentation;

        public PowerPointControl()
        {
        }

        public void PreparePresentation(String fileName)
        {
            if (presentation != null) ClosePresentation();

            application = new Application();
            presentation = application.Presentations.Open2007(fileName, Microsoft.Office.Core.MsoTriState.msoTrue, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue);
        }

        public void StartPresentation()
        {
            SlideShowSettings sst = presentation.SlideShowSettings;
            sst.ShowType = Microsoft.Office.Interop.PowerPoint.PpSlideShowType.ppShowTypeSpeaker;
            sst.ShowWithAnimation = MsoTriState.msoFalse;
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
            try
            {
                presentation.Close();
                Marshal.FinalReleaseComObject(presentation);

                application.Quit();
                Marshal.FinalReleaseComObject(application);
                GC.Collect();
            }
            catch (Exception e)
            {
            }

            application = null;
            presentation = null;
        }

        public void SaveSlidesAsPNG(string directory)
        {
            for (int i=1; i<=presentation.Slides.Count; i++)
            {
                Slide pptSlide = presentation.Slides[i];
                pptSlide.Export(directory + i.ToString() + ".png", "PNG", 1024, 768);
            }
        }

        public Boolean IsActive()
        {
            if (presentation != null) return true;
            return false;
        }
    }
}
