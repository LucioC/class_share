using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using CommonUtils;
using KinectPowerPointControl.Gesture;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using KinectPowerPointControl.Speech;
using Microsoft.Speech.Recognition;
using System.Windows.Threading;
using Microsoft.Speech.AudioFormat;
using System.Diagnostics;
using Microsoft.Kinect.Toolkit.Interaction;
using System.Diagnostics.CodeAnalysis;

namespace KinectPowerPointControl
{
    public class KinectControl
    {
        KinectSensor sensor;

        private AbstractKinectGestureRecognition gestureRecognition;
        public AbstractKinectGestureRecognition GestureRecognition { 
            get { return this.gestureRecognition; } 
            set { 
                    this.gestureRecognition = value;
                    if (this.gestureRecognition != null && this.kinectInteraction != null) this.kinectInteraction.GestureRecognition = this.gestureRecognition;
                } 
        }

        //For Image 
        byte[] colorBytes;

        public delegate void ColorFrameReadyEvent(ImageSource image);
        public event ColorFrameReadyEvent ColorFrameGot;

        public delegate void SkeletonReadyEvent(Skeleton skeleton);
        public event SkeletonReadyEvent SkeletonRecognized;

        //Speech Recognition
        private DispatcherTimer readyTimer;
        private SpeechRecognitionEngine speechRecognizer;
        public delegate void SpeechRecognizedEvent(String speech);
        public event SpeechRecognizedEvent SpeechRecognized;
        public ISpeechGrammar SpeechGrammar { get; set; }

        private Skeleton[] skeletons;

        private KinectInteractionEvents kinectInteraction;
        
        public IInteractionClient InteractionClient { get; set; }
        public KinectControl()
        {
            SpeechGrammar = null;
            GestureRecognition = null;
            this.kinectInteraction = new KinectInteractionEvents();
        }

        public KinectControl(AbstractKinectGestureRecognition gestureRecognition, ISpeechGrammar speechGrammar)
        {
            this.GestureRecognition = gestureRecognition;
            this.SpeechGrammar = speechGrammar;
            this.kinectInteraction = new KinectInteractionEvents(GestureRecognition);
        }

        public void StartKinect()
        {
            sensor = KinectSensor.KinectSensors.FirstOrDefault();

            if (sensor == null)
            {
                throw new KinectNotFoundException();
            }

            StartImageSensor();

            StartInteractionProcessing();
        }

        private InteractionStream interactionStream;
        protected void StartInteractionProcessing()
        {
            this.InteractionClient = new InteractionClient();
            this.interactionStream = new InteractionStream(this.sensor, this.InteractionClient);

            //Prepare interaction
            this.kinectInteraction.AllocateUserInfos(InteractionFrame.UserInfoArrayLength);
            this.interactionStream.InteractionFrameReady += kinectInteraction.InteractionFrameReady;
        }

        protected void StartImageSensor()
        {
            sensor.Start();

            sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
            sensor.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(ColorFrameReady);

            sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
            sensor.SkeletonStream.Enable();
            sensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(SkeletonFrameReady);

            sensor.DepthFrameReady += this.SensorDepthFrameReady;

            sensor.ElevationAngle = 0;
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

        public void InitializeSpeechRecognition()
        {
            if (SpeechGrammar == null) return;

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

            var phrases = new Choices();

            foreach (String word in SpeechGrammar.GrammarWords())
            {
                phrases.Add(word);
            }

            var gb = new GrammarBuilder();
            //Specify the culture to match the recognizer in case we are running in a different culture.                                 
            gb.Culture = ri.Culture;
            gb.Append(phrases);

            // Create the actual Grammar instance, and then load it into the speech recognizer.
            var g = new Grammar(gb);

            speechRecognizer.LoadGrammar(g);
            speechRecognizer.SpeechRecognized += SreSpeechRecognized;
            speechRecognizer.SpeechHypothesized += SreSpeechHypothesized;
            speechRecognizer.SpeechRecognitionRejected += SreSpeechRecognitionRejected;

            this.readyTimer = new DispatcherTimer();
            this.readyTimer.Tick += this.ReadyTimerTick;
            this.readyTimer.Interval = new TimeSpan(0, 0, 4);
            this.readyTimer.Start();
        }

        //when color frame is ready
        protected void ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (var image = e.OpenColorImageFrame())
            {
                if (image == null)
                    return;

                if (colorBytes == null ||
                    colorBytes.Length != image.PixelDataLength)
                {
                    colorBytes = new byte[image.PixelDataLength];
                }

                image.CopyPixelDataTo(colorBytes);

                //You could use PixelFormats.Bgr32 below to ignore the alpha,
                //or if you need to set the alpha you would loop through the bytes 
                //as in this loop below
                int length = colorBytes.Length;
                for (int i = 0; i < length; i += 4)
                {
                    colorBytes[i + 3] = 255;
                }

                BitmapSource source = BitmapSource.Create(image.Width,
                    image.Height,
                    96,
                    96,
                    PixelFormats.Bgra32,
                    null,
                    colorBytes,
                    image.Width * image.BytesPerPixel);

                if (this.ColorFrameGot != null)
                {
                    this.ColorFrameGot(source);
                }
                //videoImage.Source = source;
            }
        }

        protected void SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (var skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame == null)
                    return;
                
                //Get skeletons from this frame
                if (skeletons == null ||
                        skeletons.Length != skeletonFrame.SkeletonArrayLength)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                }
                skeletonFrame.CopySkeletonDataTo(skeletons);
                
                //Process sksletons using the custom gesture recognition
                GestureRecognition.ProcessFrameReady(skeletons);

                if (this.SkeletonRecognized != null && GestureRecognition.BestSkeleton != null)
                {
                    this.SkeletonRecognized(GestureRecognition.BestSkeleton);
                }

                //Pass info to the interaction stream               
                var accelerometerReading = this.sensor.AccelerometerGetCurrentReading();

                // Hand data to Interaction framework to be processed
                if(this.interactionStream != null)
                    this.interactionStream.ProcessSkeleton(this.skeletons, accelerometerReading, skeletonFrame.Timestamp);
               
            }                    
        }

        public ColorImagePoint MapSkeletonPointToColor(Joint joint)
        {
            return sensor.MapSkeletonPointToColor(joint.Position, sensor.ColorStream.Format);
        }

        public void StopSensor()
        {
            if (sensor != null)
            {
                sensor.AudioSource.Stop();
                sensor.Stop();
                sensor.Dispose();
                sensor = null;
            }
        }
        
        public void StopGestureRecognition()
        {

        }

        private void ReadyTimerTick(object sender, EventArgs e)
        {
            this.StartSpeechRecognition();
            this.readyTimer.Stop();
            this.readyTimer = null;
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
                this.SpeechRecognized(e.Result.Text);
            }
        }

        private void StartSpeechRecognition()
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

        public void StopSpeechRecognition()
        {
            speechRecognizer.RecognizeAsyncCancel();
            speechRecognizer.RecognizeAsyncStop();
        }        

        /// <summary>
        /// Handler for the Kinect sensor's DepthFrameReady event
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="depthImageFrameReadyEventArgs">event arguments</param>
        private void SensorDepthFrameReady(object sender, DepthImageFrameReadyEventArgs depthImageFrameReadyEventArgs)
        {
            // Even though we un-register all our event handlers when the sensor
            // changes, there may still be an event for the old sensor in the queue
            // due to the way the KinectSensor delivers events.  So check again here.
            if (this.sensor != sender)
            {
                return;
            }

            using (DepthImageFrame depthFrame = depthImageFrameReadyEventArgs.OpenDepthImageFrame())
            {
                if (null != depthFrame)
                {
                    // Hand data to Interaction framework to be processed
                    if(this.interactionStream != null)
                        this.interactionStream.ProcessDepth(depthFrame.GetRawPixelData(), depthFrame.Timestamp);                    
                }
            }
        }
    }
}
