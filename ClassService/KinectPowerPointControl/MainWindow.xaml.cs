using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using Microsoft.Speech.Recognition;
using System.Threading;
using System.IO;
using Microsoft.Speech.AudioFormat;
using System.Diagnostics;
using System.Windows.Threading;
using CommonUtils;
using KinectPowerPointControl.Speech;
using KinectPowerPointControl.Gesture;

namespace KinectPowerPointControl
{
    public partial class MainWindow : Window
    {
        KinectSensor sensor;

        ClassKinectSpeechRecognition speechRecognition;
        ISpeechGrammar grammar;

        public enum PRESENTATION_MODE { POWERPOINT, IMAGE };

        public PRESENTATION_MODE mode { get; set; }

        ClassKinectGestureRecognition gestureRecognition;

        byte[] colorBytes;
        
        bool isCirclesVisible = true;

        SolidColorBrush activeBrush = new SolidColorBrush(Colors.Green);
        SolidColorBrush inactiveBrush = new SolidColorBrush(Colors.Red);
        
        public MainWindow()
        {
            InitializeComponent();

            //Runtime initialization is handled when the window is opened. When the window
            //is closed, the runtime MUST be unitialized.
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            //Handle the content obtained from the video camera, once received.

            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);

            gestureRecognition = new ClassKinectGestureRecognition();
            gestureRecognition.GestureRecognized += this.GestureRecognized;

            mode = PRESENTATION_MODE.POWERPOINT;
        }

        public MainWindow(PRESENTATION_MODE mode)
        {
            this.mode = mode;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            sensor = KinectSensor.KinectSensors.FirstOrDefault();

            if (sensor == null)
            {
                //MessageBox.Show("This application requires a Kinect sensor.");
                Output.WriteToDebugOrConsole("A kinect sensor was not detected, closing kinect window.");
                this.Close();
                return;
            }
                        
            sensor.Start();

            sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
            sensor.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(sensor_ColorFrameReady);

            sensor.DepthStream.Enable(DepthImageFormat.Resolution320x240Fps30);
            sensor.SkeletonStream.Enable();
            sensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(sensor_SkeletonFrameReady);

            sensor.ElevationAngle = 0;

            //Application.Current.Exit += new ExitEventHandler(Current_Exit);
            this.Closed += Current_Exit;

            speechRecognition = new ClassKinectSpeechRecognition(this.sensor);
            speechRecognition.SpeechRecognized += this.SpeechRecognized;

            if (mode == PRESENTATION_MODE.POWERPOINT)
            {
                grammar = new PowerPointGrammar();
            }
            else if (mode == PRESENTATION_MODE.IMAGE)
            {
                grammar = new ImagePresentationGrammar();
            }

            speechRecognition.InitializeSpeechRecognition(GetKinectRecognizer(), grammar);
        }

        void Current_Exit(object sender, System.EventArgs e)
        {
            if (speechRecognition != null)
            {
                speechRecognition.stop();
            }
            if (sensor != null)
            {
                sensor.AudioSource.Stop();
                sensor.Stop();
                sensor.Dispose();
                sensor = null;
            }
        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C)
            {
                ToggleCircles();
            }
        }

        //when color frame is ready
        void sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
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
                videoImage.Source = source;
            }
        }


        void sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (var skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame == null)
                    return;

                gestureRecognition.ProcessFrameReady(skeletonFrame);

                //Update Right hand, left hand, and head positions for tracking and image 
                SetEllipsePosition(ellipseHead, gestureRecognition.Head, false);

                //Original version change color when a gesture is active (last parameter true)
                SetEllipsePosition(ellipseLeftHand, gestureRecognition.LeftHand, false);
                SetEllipsePosition(ellipseRightHand, gestureRecognition.RightHand, false);
                SetEllipsePosition(ellipseCenterShoulder, gestureRecognition.CenterShoulder, false);
            }
        }

        private void GestureRecognized(String gesture)
        {
            if (gesture == GestureEvents.SWIPE_RIGHT)
            {
                ProcessNextSlide();
            }
            else if (gesture == GestureEvents.SWIPE_LEFT)
            {
                ProcessPreviousSlide();
            }
            else if (gesture == GestureEvents.ZOOM_IN)
            {
                ProcessZoomIn();
            }
            else if (gesture == GestureEvents.ZOOM_OUT)
            {
                ProcessZoomOut();
            }
        }

        private void SpeechRecognized(String speech)
        {
            if (grammar.IsCommand(PowerPointGrammar.SHOW_WINDOW, speech))
            {
                this.Dispatcher.BeginInvoke((Action)delegate
                {
                    this.Topmost = true;
                    this.WindowState = System.Windows.WindowState.Normal;
                });
            }
            else if (grammar.IsCommand(PowerPointGrammar.HIDE_WINDOW, speech))
            {
                this.Dispatcher.BeginInvoke((Action)delegate
                {
                    this.Topmost = false;
                    this.WindowState = System.Windows.WindowState.Minimized;
                });
            }
            else if (grammar.IsCommand(PowerPointGrammar.HIDE_CIRCLES, speech))
            {
                this.Dispatcher.BeginInvoke((Action)delegate
                {
                    this.HideCircles();
                });
            }
            else if (grammar.IsCommand(PowerPointGrammar.SHOW_CIRCLES, speech))
            {
                this.Dispatcher.BeginInvoke((Action)delegate
                {
                    this.ShowCircles();
                });
            }
            else if (grammar.IsCommand(PowerPointGrammar.NEXT_SLIDE, speech))
            {
                ProcessNextSlide();
            }
            else if (grammar.IsCommand(PowerPointGrammar.PREVIOUS_SLIDE, speech))
            {
                ProcessPreviousSlide();
            }
            else if (grammar.IsCommand(PowerPointGrammar.CLOSE_PRESENTATION, speech))
            {
                ProcessClosePresentation();
            }
            else if (grammar.IsCommand(ImagePresentationGrammar.MOVE_RIGHT, speech))
            {
                ProcessMoveRight();
            }
            else if (grammar.IsCommand(ImagePresentationGrammar.MOVE_LEFT, speech))
            {
                ProcessMoveLeft();
            }
            else if (grammar.IsCommand(ImagePresentationGrammar.MOVE_UP, speech))
            {
                ProcessMoveLeft();
            }
            else if (grammar.IsCommand(ImagePresentationGrammar.MOVE_DOWN, speech))
            {
                ProcessMoveDown();
            }
            else if (grammar.IsCommand(ImagePresentationGrammar.ROTATE_RIGHT, speech))
            {
                ProcessRotateRight();
            }
            else if (grammar.IsCommand(ImagePresentationGrammar.ROTATE_LEFT, speech))
            {
                ProcessRotateLeft();
            }
            else if (grammar.IsCommand(ImagePresentationGrammar.ZOOM_IN, speech))
            {
                ProcessZoomIn();
            }
            else if (grammar.IsCommand(ImagePresentationGrammar.ZOOM_OUT, speech))
            {
                ProcessZoomOut();
            }
            else if (grammar.IsCommand(ImagePresentationGrammar.CLOSE_IMAGE, speech))
            {
                ProcessCloseImage();
            }
        }

        public void ProcessClosePresentation()
        {
            System.Windows.Forms.SendKeys.SendWait("{ESC}");
            this.Close();
        }

        public void ProcessCloseImage()
        {
            System.Windows.Forms.SendKeys.SendWait("{ESC}");
            this.Close();
        }

        #region imageControl
        
        private void ProcessMoveRight()
        {
            System.Windows.Forms.SendKeys.SendWait("{Right}");
        }

        private void ProcessMoveLeft()
        {
            System.Windows.Forms.SendKeys.SendWait("{Left}");
        }

        private void ProcessMoveUp()
        {
            System.Windows.Forms.SendKeys.SendWait("{UP}");
        }

        private void ProcessMoveDown()
        {
            System.Windows.Forms.SendKeys.SendWait("{Down}");
        }

        private void ProcessRotateRight()
        {
            System.Windows.Forms.SendKeys.SendWait("{END}");
        }

        private void ProcessRotateLeft()
        {
            System.Windows.Forms.SendKeys.SendWait("{HOME}");
        }
            
        private void ProcessZoomOut()
        {
            System.Windows.Forms.SendKeys.SendWait("{PGDN}");
        }
        
        private void ProcessZoomIn()
        {
            System.Windows.Forms.SendKeys.SendWait("{PGUP}");
        }

        #endregion

        #region slideControl

        private void ProcessNextSlide()
        {
            System.Windows.Forms.SendKeys.SendWait("{Right}");
        }

        private void ProcessPreviousSlide()
        {
            System.Windows.Forms.SendKeys.SendWait("{Left}");
        }

        #endregion

        //This method is used to position the ellipses on the canvas
        //according to correct movements of the tracked joints.
        private void SetEllipsePosition(Ellipse ellipse, Joint joint, bool isHighlighted)
        {
            var point = sensor.MapSkeletonPointToColor(joint.Position, sensor.ColorStream.Format);

            if (isHighlighted)
            {
                ellipse.Width = 60;
                ellipse.Height = 60;
                ellipse.Fill = activeBrush;
            }
            else
            {
                ellipse.Width = 20;
                ellipse.Height = 20;
                ellipse.Fill = inactiveBrush;
            }

            Canvas.SetLeft(ellipse, point.X - ellipse.ActualWidth / 2);
            Canvas.SetTop(ellipse, point.Y - ellipse.ActualHeight / 2);
        }

        void ToggleCircles()
        {
            if (isCirclesVisible)
                HideCircles();
            else
                ShowCircles();
        }

        void HideCircles()
        {
            isCirclesVisible = false;
            ellipseHead.Visibility = System.Windows.Visibility.Collapsed;
            ellipseLeftHand.Visibility = System.Windows.Visibility.Collapsed;
            ellipseRightHand.Visibility = System.Windows.Visibility.Collapsed;
            ellipseCenterShoulder.Visibility = System.Windows.Visibility.Collapsed;
        }

        void ShowCircles()
        {
            isCirclesVisible = true;
            ellipseHead.Visibility = System.Windows.Visibility.Visible;
            ellipseLeftHand.Visibility = System.Windows.Visibility.Visible;
            ellipseRightHand.Visibility = System.Windows.Visibility.Visible;
            ellipseCenterShoulder.Visibility = System.Windows.Visibility.Visible;
        }

        #region Speech Recognition Methods

        private static RecognizerInfo GetKinectRecognizer()
        {
            Func<RecognizerInfo, bool> matchingFunc = r =>
            {
                string value;
                r.AdditionalInfo.TryGetValue("Kinect", out value);
                return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase) && "en-US".Equals(r.Culture.Name, StringComparison.InvariantCultureIgnoreCase);
            };
            return SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).FirstOrDefault();
        }

        
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

    }
}
