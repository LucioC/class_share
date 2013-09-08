﻿using System;
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
using KinectPowerPointControl.Speech;
using KinectPowerPointControl.Gesture;
using ServiceCore;
using ServiceCore.Utils;

namespace KinectPowerPointControl
{
    public partial class KinectControlWindow : Window
    {
        KinectControl kinectControl;
        AbstractKinectGestureRecognition gestureRecognition;
        Grammar grammar;

        SkeletonStateRepository skeletonRepository { get; set; }

        public event MessageEvent MessageSent;

        public PRESENTATION_MODE mode { get; set; }

        bool isCirclesVisible = true;

        KinectSpeechControl speechControl;

        SolidColorBrush activeBrush = new SolidColorBrush(Colors.Yellow);
        SolidColorBrush inactiveBrush = new SolidColorBrush(Colors.DarkRed);

        private ServiceCommandsLocalActivation commands;

        public KinectControlWindow():this(PRESENTATION_MODE.IMAGE)
        {
            
        }

        public KinectControlWindow(PRESENTATION_MODE mode)
        {
            InitializeComponent();
            setWindowPosition();

            skeletonRepository = new SkeletonStateRepository();
            commands = new ServiceCommandsLocalActivation(MessageSent);

            //Runtime initialization is handled when the window is opened. When the window
            //is closed, the runtime MUST be unitialized.
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            //Handle the content obtained from the video camera, once received.
            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);

            this.mode = mode;

            CreateGestureRecognition(mode);

            Minimize();
            UnMinimize();
        }

        private void setWindowPosition()
        {
            int top = (int)(System.Windows.SystemParameters.PrimaryScreenHeight - this.Height);
            int left = (int)(System.Windows.SystemParameters.PrimaryScreenWidth - this.Width);
            this.Top = top;
            this.Left = left;
        }

        private void CreateGestureRecognition(PRESENTATION_MODE mode)
        {
            if (mode == PRESENTATION_MODE.POWERPOINT)
            {
                gestureRecognition = new PowerPointKinectGestureRecognition(skeletonRepository);
            }
            else if (mode == PRESENTATION_MODE.IMAGE)
            {
                gestureRecognition = new ImagePresentationKinectGestureRecognition(skeletonRepository);
            }
            gestureRecognition.GestureRecognized += this.GestureRecognized;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            kinectControl = new KinectControl(gestureRecognition, skeletonRepository);

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

            speechControl = kinectControl.ReturnSpeechControl();
            grammar = createGrammar(mode);
            speechControl.InitializeSpeechRecognition(grammar);
            speechControl.SpeechRecognized += this.SpeechRecognized;
            speechControl.SpeechHypothesized += this.SpeechHypothesized;

            commands.setListeners(this.MessageSent);

        }

        private Grammar createGrammar(PRESENTATION_MODE mode)
        {
            Grammar grammar = null;
            if (mode == PRESENTATION_MODE.IMAGE)
            {
                grammar = speechControl.CreateGrammarFromResource(Properties.Resources.image_presentation);
            }
            else if (mode == PRESENTATION_MODE.POWERPOINT)
            {
                grammar = speechControl.CreateGrammarFromResource(Properties.Resources.slide_presentation);
            }

            return grammar;
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
            SetEllipsePosition(ellipseLeftHand, LeftHand, this.skeletonRepository.FirstUser.IsLeftHandGripped);
            SetEllipsePosition(ellipseRightHand, RightHand, this.skeletonRepository.FirstUser.IsRightHandGripped);
            
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
            speechControl.UnloadGrammar(grammar);
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
                commands.ProcessNextSlide();
            }
            else if (gesture == GestureEvents.SWIPE_LEFT)
            {
                commands.ProcessPreviousSlide();
            }
            else if (gesture == GestureEvents.ZOOM_IN)
            {
                commands.ProcessZoomIn();
            }
            else if (gesture == GestureEvents.ZOOM_OUT)
            {
                commands.ProcessZoomOut();
            }
            else if (gesture == GestureEvents.MOVE_RIGHT)
            {
                commands.ProcessMoveRight();
            }
            else if (gesture == GestureEvents.MOVE_LEFT)
            {
                commands.ProcessMoveLeft();
            }
            else if (gesture == GestureEvents.MOVE_UP)
            {
                commands.ProcessMoveUp();
            }
            else if (gesture == GestureEvents.MOVE_DOWN)
            {
                commands.ProcessMoveDown();
            }
            else if (gesture == GestureEvents.ROTATE_RIGHT)
            {
                commands.ProcessRotateRight();
            }
            else if (gesture == GestureEvents.ROTATE_LEFT)
            {
                commands.ProcessRotateLeft();
            }
            else if (gesture == GestureEvents.JOIN_HANDS)
            {
                //FIXME closing both, may need to verify which is being used and close it only
                commands.ProcessCloseImage();
                commands.ProcessClosePresentation();
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

        private void SpeechHypothesized(RecognitionResult speechHypothesized)
        {
            ListBoxItem item = createDefaultListBoxItem();
            item.Content = "S:" + speechHypothesized.Text + " C:" + speechHypothesized.Confidence;
            addToList(item);
        }

        private ListBoxItem createDefaultListBoxItem()
        {
            ListBoxItem item = new ListBoxItem();
            item.FontSize = 28;
            return item;
        }

        private void addToList(ListBoxItem item)
        {
            listBox1.Items.Add(item);

            if (listBox1.Items.Count > 3) listBox1.Items.RemoveAt(0);
        }

        private void SpeechRecognized(RecognitionResult speechRecognized)
        {
            SemanticValue semanticValue = speechRecognized.Semantics["command"];

            if (!skeletonRepository.FirstUser.IsFacingForward)
            {
                Output.Debug("KinectControlWindow","User not facing forward, speech ignored.");
                return;
            }

            this.listBox1.Items.Clear(); 
            ListBoxItem item = createDefaultListBoxItem();
            item.Content = "S:" + speechRecognized.Text + " C:" + speechRecognized.Confidence;
            addToList(item);
            
            string speech = (string)semanticValue.Value;

            if (PowerPointGrammar.NEXT_SLIDE.Equals(speech))
            {
                commands.ProcessNextSlide();
            }
            else if (PowerPointGrammar.PREVIOUS_SLIDE.Equals(speech))
            {
                commands.ProcessPreviousSlide();
            }
            else if (PowerPointGrammar.CLOSE_PRESENTATION.Equals(speech))
            {
                commands.ProcessClosePresentation();
            }
            else if (ImagePresentationGrammar.MOVE_RIGHT.Equals(speech))
            {
                commands.ProcessMoveRight();
            }
            else if (ImagePresentationGrammar.MOVE_LEFT.Equals(speech))
            {
                commands.ProcessMoveLeft();
            }
            else if (ImagePresentationGrammar.MOVE_UP.Equals(speech))
            {
                commands.ProcessMoveUp();
            }
            else if (ImagePresentationGrammar.MOVE_DOWN.Equals(speech))
            {
                commands.ProcessMoveDown();
            }
            else if (ImagePresentationGrammar.ROTATE_RIGHT.Equals(speech))
            {
                commands.ProcessRotateRight();
            }
            else if (ImagePresentationGrammar.ROTATE_LEFT.Equals(speech))
            {
                commands.ProcessRotateLeft();
            }
            else if (ImagePresentationGrammar.ZOOM_IN.Equals(speech))
            {
                commands.ProcessZoomIn();
            }
            else if (ImagePresentationGrammar.ZOOM_OUT.Equals(speech))
            {
                commands.ProcessZoomOut();
            }
            else if (ImagePresentationGrammar.CLOSE_IMAGE.Equals(speech))
            {
                commands.ProcessCloseImage();
            }
        }

        //This method is used to position the ellipses on the canvas
        //according to correct movements of the tracked joints.
        private void SetEllipsePosition(Ellipse ellipse, Joint joint, bool isHighlighted)
        {
            var point = kinectControl.MapSkeletonPointToColor(joint);

            if (isHighlighted)
            {
                ellipse.Width = 30;
                ellipse.Height = 30;
                ellipse.Fill = activeBrush;
            }
            else
            {
                ellipse.Width = 15;
                ellipse.Height = 15;
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

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}
