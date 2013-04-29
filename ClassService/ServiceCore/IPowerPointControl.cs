using System;

namespace ServiceCore
{
    public enum PRESENTATION_MODE { FILE_SELECTION, POWERPOINT, IMAGE };

    public interface IPowerPointControl
    {
        void ClosePresentation();
        int CurrentSlide();
        void GoToFirstSlide();
        void GoToLastSlide();
        void GoToNextSlide();
        void GoToPreviousSlide();
        void GoToSlideNumber(int slideNumber);
        void PreparePresentation(string fileName);
        void StartPresentation();
        int TotalSlides();
    }
}
