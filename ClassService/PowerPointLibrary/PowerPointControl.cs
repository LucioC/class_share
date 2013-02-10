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

        public Boolean PreparePresentation(String fileName)
        {
            application = new Application();
            try
            {
                presentation = application.Presentations.Open2007(fileName, Microsoft.Office.Core.MsoTriState.msoTrue, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue);
                writeToConsoleAndOutput("Open presentation");
                
                return true;
            }
            catch (Exception e)
            {
                writeToConsoleAndOutput("Power point open returned a exception message: " + e.Message);
                return false;
            }
        }

        public void StartPresentation()
        {
            SlideShowSettings sst = presentation.SlideShowSettings;
            sst.ShowType = Microsoft.Office.Interop.PowerPoint.PpSlideShowType.ppShowTypeSpeaker;
            sst.Run();
            writeToConsoleAndOutput("Started presentation");
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
            writeToConsoleAndOutput("Next slide command. curent slide position: " + presentation.SlideShowWindow.View.CurrentShowPosition.ToString());
        }

        public void GoToPreviousSlide()
        {
            presentation.SlideShowWindow.View.Previous();
            writeToConsoleAndOutput("Previous slide command. curent slide position: " + presentation.SlideShowWindow.View.CurrentShowPosition.ToString());
        }

        public int TotalSlides()
        {
            return presentation.Slides.Count;
        }

        public Boolean ClosePresentation()
        {
            try
            {
                presentation.Close();
                return true;
            }
            catch (Exception e)
            {
                writeToConsoleAndOutput("Power point close returned a exception message: " + e.Message);
                return false;
            }
        }

        public void writeToConsoleAndOutput(String message)
        {
            System.Diagnostics.Debug.Write("\n\n" + message + "\n\n");
            Console.WriteLine("\n\n" + message + "\n\n");
        }
    }
}
