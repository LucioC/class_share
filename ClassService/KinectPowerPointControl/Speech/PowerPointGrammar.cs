using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectPowerPointControl.Speech
{
    public class PowerPointGrammar: AbstractGrammar
    {
        public const String NEXT_SLIDE = "next slide";
        public const String PREVIOUS_SLIDE = "previous slide";
        public const String START_PRESENTATION = "start presentation";
        public const String CLOSE_PRESENTATION = "close presentation";

        public const String SHOW_WINDOW = "computer show window";
        public const String HIDE_WINDOW = "computer hide window";
        public const String SHOW_CIRCLES = "computer show circles";
        public const String HIDE_CIRCLES = "computer hide circles";

        public PowerPointGrammar()
        {
            words = new List<string>();
            dictionary = new Dictionary<string, IList<string>>();

            //FIXME could take sentences from a file?
            //NEXT SLIDE COMMAND
            List<String> nextSlide = new List<string>();
            nextSlide.Add("next slide");
            nextSlide.Add("next");
            nextSlide.Add("forward");
            dictionary.Add(NEXT_SLIDE, nextSlide);

            //PREVIOUS SLIDE COMMAND
            List<String> previousSlide = new List<string>();
            previousSlide.Add("previous slide");
            previousSlide.Add("previous");
            previousSlide.Add("back");
            dictionary.Add(PREVIOUS_SLIDE, previousSlide);

            //START PRESENTATION COMMAND
            List<String> startPresentation = new List<string>();
            startPresentation.Add("start presentation");
            startPresentation.Add("open presentation");
            dictionary.Add(START_PRESENTATION, startPresentation);

            //CLOSE PRESENTATION COMMAND
            List<String> closePresentation = new List<string>();
            closePresentation.Add("close presentation");
            closePresentation.Add("close");
            dictionary.Add(CLOSE_PRESENTATION, closePresentation);
            
            //SHOW WINDOW COMMAND
            List<String> showWindow = new List<string>();
            showWindow.Add("computer show window");
            dictionary.Add(SHOW_WINDOW, showWindow);

            //HIDE WINDOW COMMAND
            List<String> hideWindow = new List<string>();
            hideWindow.Add("computer hide window");
            dictionary.Add(HIDE_WINDOW, hideWindow);

            //SHOW WINDOW COMMAND
            List<String> showCircles = new List<string>();
            showCircles.Add("computer show circles");
            dictionary.Add(SHOW_CIRCLES, showCircles);

            //SHOW WINDOW COMMAND
            List<String> hideCircles = new List<string>();
            hideCircles.Add("computer hide circles");
            dictionary.Add(HIDE_CIRCLES, hideCircles);

            words.AddRange(nextSlide);
            words.AddRange(previousSlide);
            words.AddRange(startPresentation);
            words.AddRange(closePresentation);

            words.AddRange(showWindow);
            words.AddRange(hideWindow);
            words.AddRange(showCircles);
            words.AddRange(hideCircles);
        }
    }
}
