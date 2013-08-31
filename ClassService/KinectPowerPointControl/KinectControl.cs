using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
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
using ServiceCore.Utils;
using ServiceCore;

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
        public ISpeechGrammar SpeechGrammar { get; set; }

        private Skeleton[] skeletons;

        private KinectInteractionEvents kinectInteraction;
        
        public IInteractionClient InteractionClient { get; set; }

        public SkeletonStateRepository UserStateRepository { get; set; }

        public KinectControl(AbstractKinectGestureRecognition gestureRecognition, ISpeechGrammar speechGrammar, SkeletonStateRepository skeletonRepository)
        {
            this.GestureRecognition = gestureRecognition;
            this.SpeechGrammar = speechGrammar;
            this.UserStateRepository = skeletonRepository;
            this.kinectInteraction = new KinectInteractionEvents(GestureRecognition, UserStateRepository);
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

        public KinectSpeechControl ReturnSpeechControl()
        {
            return new KinectSpeechControl(this.sensor);
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
            sensor.ColorFrameReady += this.ColorFrameReady;

            sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
            sensor.SkeletonStream.Enable();
            sensor.SkeletonFrameReady += this.SkeletonFrameReady;

            sensor.DepthFrameReady += this.SensorDepthFrameReady;

            sensor.ElevationAngle = 0;
        }

        public void StopListeningKinect()
        {
            sensor.DepthFrameReady -= this.SensorDepthFrameReady;
            sensor.SkeletonFrameReady -= this.SkeletonFrameReady;
            sensor.ColorFrameReady -= this.ColorFrameReady;
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
            if (this.sensor != null)
            {
                return sensor.MapSkeletonPointToColor(joint.Position, sensor.ColorStream.Format);
            }
            else
            {
                return new ColorImagePoint();
            }
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
