using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using Microsoft.Speech.Recognition;
using KinectPowerPointControl.Speech;
using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
using ServiceCore.Utils;
using System.Diagnostics;
using System.IO;

namespace KinectPowerPointControl
{
    public class KinectSpeechControl
    {
        private DispatcherTimer readyTimer;
        private SpeechRecognitionEngine speechRecognizer;
        private KinectSensor sensor;
        public delegate void SpeechRecognizedEvent(RecognitionResult speech);
        public event SpeechRecognizedEvent SpeechRecognized;

        public KinectSpeechControl(KinectSensor sensor)
        {
            this.sensor = sensor;
        }

        protected static RecognizerInfo GetKinectRecognizer()
        {
            Func<RecognizerInfo, bool> matchingFunc = r =>
            {
                string value;
                r.AdditionalInfo.TryGetValue("Kinect", out value);
                return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase) && "en-US".Equals(r.Culture.Name, StringComparison.InvariantCultureIgnoreCase);
            };
            return SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).FirstOrDefault();
        }
        
        public Grammar CreateGrammarFromResource(string resource)
        {
            Grammar grammar;

            using (var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(resource)))  //Access a Gramar File
            {
                grammar = new Grammar(memoryStream);
            }

            return grammar;
        }

        public Grammar CreateGrammar(ISpeechGrammar grammar)
        {
            var phrases = new Choices();

            foreach (String word in grammar.GrammarWords())
            {
                phrases.Add(word);
            }

            RecognizerInfo recognizer = GetKinectRecognizer();
            RecognizerInfo ri = recognizer;

            var gb = new GrammarBuilder();
            //Specify the culture to match the recognizer in case we are running in a different culture.                                 
            gb.Culture = ri.Culture;
            gb.Append(phrases);

            // Create the actual Grammar instance, and then load it into the speech recognizer.
            var g = new Grammar(gb);
            return g;
        }

        public void InitializeSpeechRecognition(Grammar grammar)
        {
            if (grammar == null) return;

            RecognizerInfo recognizer = GetKinectRecognizer();
            RecognizerInfo ri = recognizer;
            if (ri == null)
            {
                throw new KinectSpeechRecognitionInitializationException();
            }

            try
            {
                speechRecognizer = new SpeechRecognitionEngine(ri.Id);
            }
            catch
            {
                throw new KinectSpeechRecognitionInitializationException();
            }

            speechRecognizer.LoadGrammar(grammar);
            speechRecognizer.SpeechRecognized += SreSpeechRecognized;
            speechRecognizer.SpeechHypothesized += SreSpeechHypothesized;
            speechRecognizer.SpeechRecognitionRejected += SreSpeechRecognitionRejected;

            this.readyTimer = new DispatcherTimer();
            this.readyTimer.Tick += this.ReadyTimerTick;
            this.readyTimer.Interval = new TimeSpan(0, 0, 4);
            this.readyTimer.Start();
        }
        
        public void StopSpeechRecognition()
        {
            speechRecognizer.RecognizeAsyncCancel();
            speechRecognizer.RecognizeAsyncStop();
        }        


        private void ReadyTimerTick(object sender, EventArgs e)
        {
            this.StartSpeechRecognition(this.sensor);
            this.readyTimer.Stop();
            this.readyTimer = null;
        }
        
        private void StartSpeechRecognition(KinectSensor sensor)
        {
            if (sensor == null || speechRecognizer == null)
                return;

            try
            {
                var audioSource = this.sensor.AudioSource;
                audioSource.BeamAngleMode = BeamAngleMode.Adaptive;
                var kinectStream = audioSource.Start();

                speechRecognizer.SetInputToAudioStream(
                        kinectStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechRecognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception e)
            {
                Output.WriteToDebugOrConsole(e.Message);
            }

        }

        void SreSpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            Trace.WriteLine("\nSpeech Rejected, confidence: " + e.Result.Confidence);
        }

        void SreSpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            Trace.Write("\rSpeech Hypothesized: \t{0}", e.Result.Text);
        }

        void SreSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            //This first release of the Kinect language pack doesn't have a reliable confidence model, so 
            //we don't use e.Result.Confidence here.
            if (e.Result.Confidence < 0.70)
            {
                Trace.WriteLine("\nSpeech Rejected filtered, confidence: " + e.Result.Confidence);
                return;
            }

            Trace.WriteLine("\nSpeech Recognized, confidence: " + e.Result.Confidence + ": \t{0}", e.Result.Text);

            //Trigger event so the recognized text can be processed and generate some action outside of this class
            if (this.SpeechRecognized != null)
            {
                this.SpeechRecognized(e.Result);
            }
        }
    }
}
