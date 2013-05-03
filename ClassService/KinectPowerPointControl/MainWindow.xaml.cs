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
using ServiceCore;

namespace KinectPowerPointControl
{
    public partial class MainWindow : Window
    {
        KinectControl kinectControl;
        AbstractKinectGestureRecognition gestureRecognition;
        ISpeechGrammar grammar;

        public event MessageEvent MessageSent;

        public PRESENTATION_MODE mode { get; set; }
        
        bool isCirclesVisible = true;

        SolidColorBrush activeBrush = new SolidColorBrush(Colors.Green);
        SolidColorBrush inactiveBrush = new SolidColorBrush(Colors.DarkRed);

        public MainWindow():this(PRESENTATION_MODE.IMAGE)
        {

        }

        public MainWindow(PRESENTATION_MODE mode)
        {
            InitializeComponent();

            //Runtime initialization is handled when the window is opened. When the window
            //is closed, the runtime MUST be unitialized.
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            //Handle the content obtained from the video camera, once received.

            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);

            this.mode = mode;

            if (mode == PRESENTATION_MODE.POWERPOINT)
            {
                gestureRecognition = new PowerPointKinectGestureRecognition();
            }
            else if (mode == PRESENTATION_MODE.IMAGE)
            {
                gestureRecognition = new ImagePresentationKinectGestureRecognition();
            }
            gestureRecognition.GestureRecognized += this.GestureRecognized;

            Minimize();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            kinectControl = new KinectControl();

            kinectControl.GestureRecognition = gestureRecognition;
            kinectControl.ColorFrameGot += this.UpdateImage;
            kinectControl.SkeletonRecognized += SkeletonReady;

            try
            {
                kinectControl.StartKinect();
            }
            catch(KinectNotFoundException exception)
            {
                Output.WriteToDebugOrConsole(exception.Message);
                this.Close();
            }

            //Application.Current.Exit += new ExitEventHandler(Current_Exit);
            this.Closed += Current_Exit;

            kinectControl.SpeechRecognized += this.SpeechRecognized;

            if (mode == PRESENTATION_MODE.POWERPOINT)
            {
                grammar = new PowerPointGrammar();
            }
            else if (mode == PRESENTATION_MODE.IMAGE)
            {
                grammar = new ImagePresentationGrammar();
            }
            kinectControl.SpeechGrammar = grammar;

            kinectControl.InitializeSpeechRecognition();
        }

        public void SkeletonReady(Skeleton skeleton)
        {
            var Head = skeleton.Joints[JointType.Head];
            var LeftHand = skeleton.Joints[JointType.HandLeft];
            var RightHand = skeleton.Joints[JointType.HandRight];
            var RightWrist = skeleton.Joints[JointType.WristRight];
            var LeftWrist = skeleton.Joints[JointType.WristLeft];
            var CenterShoulder = skeleton.Joints[JointType.ShoulderCenter];
            var Spine = skeleton.Joints[JointType.Spine];
            var CenterHip = skeleton.Joints[JointType.HipCenter];
            var RightElbow = skeleton.Joints[JointType.ElbowRight];
            var LeftElbow = skeleton.Joints[JointType.ElbowLeft];
            var RightShoulder = skeleton.Joints[JointType.ShoulderRight];
            var LeftShoulder = skeleton.Joints[JointType.ShoulderLeft];

            //Update Right hand, left hand, and head positions for tracking and image 
            SetEllipsePosition(ellipseHead, Head, false);

            //Original version change color when a gesture is active (last parameter true)
            SetEllipsePosition(ellipseLeftHand, LeftHand, false);
            SetEllipsePosition(ellipseRightHand, RightHand, false);
            SetEllipsePosition(ellipseCenterShoulder, CenterShoulder, false);

            SetEllipsePosition(ellipseRightWrist, RightWrist, false);
            SetEllipsePosition(ellipseLeftWrist, LeftWrist, false);

            SetEllipsePosition(ellipseSpine, Spine, false);
            SetEllipsePosition(ellipseCenterHip, CenterHip, false);

            SetEllipsePosition(ellipseRightElbow, RightElbow, false);
            SetEllipsePosition(ellipseLeftElbow, LeftElbow, false);

            SetEllipsePosition(ellipseRightShoulder, RightShoulder, false);
            SetEllipsePosition(ellipseLeftShoulder, LeftShoulder, false);
        }

        protected void UpdateImage(ImageSource source)
        {
            videoImage.Source = source;
        }

        void Current_Exit(object sender, System.EventArgs e)
        {
            //kinectControl.StopSensor();
            kinectControl.StopListeningKinect();
            Application.Current.Shutdown();
        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C)
            {
                ToggleCircles();
            }
        }

        
        private void GestureRecognized(String gesture)
        {
            if (gesture == GestureEvents.SWIPE_RIGHT)
            {
                ProcessPreviousSlide();
            }
            else if (gesture == GestureEvents.SWIPE_LEFT)
            {
                ProcessNextSlide();
            }
            else if (gesture == GestureEvents.ZOOM_IN)
            {
                ProcessZoomIn();
            }
            else if (gesture == GestureEvents.ZOOM_OUT)
            {
                ProcessZoomOut();
            }
            else if (gesture == GestureEvents.MOVE_RIGHT)
            {
                ProcessMoveRight();
            }
            else if (gesture == GestureEvents.MOVE_LEFT)
            {
                ProcessMoveLeft();
            }
            else if (gesture == GestureEvents.MOVE_UP)
            {
                ProcessMoveUp();
            }
            else if (gesture == GestureEvents.MOVE_DOWN)
            {
                ProcessMoveDown();
            }
            else if (gesture == GestureEvents.ROTATE_RIGHT)
            {
                ProcessRotateRight();
            }
            else if (gesture == GestureEvents.ROTATE_LEFT)
            {
                ProcessRotateLeft();
            }
            else if (gesture == GestureEvents.JOIN_HANDS)
            {
                ProcessCloseImage();
                ProcessClosePresentation();
            }
        }

        private void Minimize()
        {
            this.Dispatcher.BeginInvoke((Action)delegate
            {
                this.Topmost = false;
                this.WindowState = System.Windows.WindowState.Minimized;
            });
        }

        private void UnMinimize()
        {
            this.Dispatcher.BeginInvoke((Action)delegate
            {
                this.Topmost = true;
                this.WindowState = System.Windows.WindowState.Normal;
            });
        }

        private void SpeechRecognized(String speech)
        {
            if (grammar.IsCommand(PowerPointGrammar.SHOW_WINDOW, speech))
            {
                UnMinimize();
            }
            else if (grammar.IsCommand(PowerPointGrammar.HIDE_WINDOW, speech))
            {
                Minimize();
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
                ProcessMoveUp();
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
            if (this.MessageSent != null)
            {
                this.MessageSent.BeginInvoke("closepresentation", null, null);
            }
        }

        public void ProcessCloseImage()
        {
            if (this.MessageSent != null)
            {
                this.MessageSent.BeginInvoke("closeimage", null, null);
            }
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
            var point = kinectControl.MapSkeletonPointToColor(joint);

            if (isHighlighted)
            {
                ellipse.Width = 60;
                ellipse.Height = 60;
                ellipse.Fill = activeBrush;
            }
            else
            {
                ellipse.Width = 10;
                ellipse.Height = 10;
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
            ellipseLeftWrist.Visibility = System.Windows.Visibility.Visible;
            ellipseRightWrist.Visibility = System.Windows.Visibility.Visible;
            ellipseSpine.Visibility = System.Windows.Visibility.Visible;
            ellipseCenterHip.Visibility = System.Windows.Visibility.Visible;
            ellipseRightElbow.Visibility = System.Windows.Visibility.Visible;
            ellipseLeftElbow.Visibility = System.Windows.Visibility.Visible;
            ellipseRightShoulder.Visibility = System.Windows.Visibility.Visible;
            ellipseLeftShoulder.Visibility = System.Windows.Visibility.Visible;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

    }
}
