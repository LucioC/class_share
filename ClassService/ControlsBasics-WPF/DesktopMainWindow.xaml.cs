//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.ControlsBasics
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using Microsoft.Kinect;
    using Microsoft.Kinect.Toolkit;
    using Microsoft.Kinect.Toolkit.Controls;
    using System.IO;
    using ServiceCore;
    using System.Windows.Input;
    using ClassService.MainWindow;
    using ServiceCore.Utils;
    using System.Linq;
    using System.Text;
    using Microsoft.Speech.Recognition;
    using System.Diagnostics;
    using Microsoft.Speech.AudioFormat;
    using KinectPowerPointControl;
    using System.Windows.Media;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class DesktopMainWindow
    {
        public static readonly DependencyProperty PageLeftEnabledProperty = DependencyProperty.Register(
            "PageLeftEnabled", typeof(bool), typeof(DesktopMainWindow), new PropertyMetadata(false));

        public static readonly DependencyProperty PageRightEnabledProperty = DependencyProperty.Register(
            "PageRightEnabled", typeof(bool), typeof(DesktopMainWindow), new PropertyMetadata(false));

        private const double ScrollErrorMargin = 0.001;

        private const int PixelScrollByAmount = 20;

        private readonly KinectSensorChooser sensorChooser;

        private SkeletonStateRepository repository;

        public event MessageEvent MessageSent;

        public string FilesFolder { get; set; }

        KinectSpeechControl speechControl;

        Grammar grammar;

        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopMainWindow"/> class. 
        /// </summary>
        public DesktopMainWindow()
        {
            this.InitializeComponent();

            // initialize the sensor chooser and UI
            this.sensorChooser = new KinectSensorChooser();
            this.sensorChooser.KinectChanged += SensorChooserOnKinectChanged;
            this.sensorChooserUi.KinectSensorChooser = this.sensorChooser;
            this.sensorChooser.Start();

            // Bind listner to scrollviwer scroll position change, and check scroll viewer position
            this.UpdatePagingButtonState();
            scrollViewer.ScrollChanged += (o, e) => this.UpdatePagingButtonState();

            this.WindowState = System.Windows.WindowState.Maximized;
            
            speechControl = new KinectSpeechControl(this.sensorChooser.Kinect);

            repository = new SkeletonStateRepository();
            this.kinectRegion.Repository = repository;

            grammar = speechControl.CreateGrammarFromResource(Properties.Resources.WindowGrammar);
            StartEvents();
        }

        public void StartEvents()
        {
            this.wrapPanel.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new System.Windows.RoutedEventHandler(this.KinectTileButtonClick));
            var regionSensorBinding = new Binding("Kinect") { Source = this.sensorChooser };
            BindingOperations.SetBinding(this.kinectRegion, KinectRegion.KinectSensorProperty, regionSensorBinding);
            speechControl.InitializeSpeechRecognition(grammar);
            speechControl.SpeechRecognized += this.SpeechRecognized;
            speechControl.SpeechHypothesized += this.SpeechHypothesized;
        }

        public void PauseEvents()
        {
            //speechControl.StopSpeechRecognition();
            this.wrapPanel.RemoveHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new System.Windows.RoutedEventHandler(this.KinectTileButtonClick));
            
            speechControl.SpeechRecognized -= this.SpeechRecognized;
            speechControl.SpeechHypothesized -= this.SpeechHypothesized;
            BindingOperations.ClearBinding(this.kinectRegion, KinectRegion.KinectSensorProperty);
            speechControl.UnloadGrammar(grammar);
            speechControl.StopEvents();
        }

        private void SpeechHypothesized(RecognitionResult speechHypothesized)
        {
            ListBoxItem item = createDefaultListBoxItem();
            item.Content = "S:" + speechHypothesized.Text + " C:" + speechHypothesized.Confidence;
            addToList(item);
        }

        private void addToList(ListBoxItem item)
        {
            listbox1.Items.Add(item);

            if (listbox1.Items.Count > 3) listbox1.Items.RemoveAt(0);
        }

        private ListBoxItem createDefaultListBoxItem()
        {
            ListBoxItem item = new ListBoxItem();
            item.FontSize = 20;
            return item;
        }

        public void SpeechRecognized(RecognitionResult speech)
        {
            if (!repository.FirstUser.IsFacingForward)
            {
                Output.Debug("DesktopMainWindow", "User is not facing forward, speech ignored.");
                return;
            }
            
            ListBoxItem item = createDefaultListBoxItem();
            item.Content = "S:" + speech.Text + " C:" + speech.Confidence;
            addToList(item);

            SemanticValue semanticValue = speech.Semantics["number"];
            int fileNumber = Int32.Parse((string)semanticValue.Value);

            if (fileNumber < this.wrapPanel.Children.Count && fileNumber >= 0)
            {
                var button = this.wrapPanel.Children[fileNumber];

                Output.Debug("SpeechRecognizedOnFileSelection", "File number chosen was " + fileNumber);

                clickButton((FileKinectButton)button);
            }
        }

        public void LoadFilesFromFolder(string folderPath)
        {
            this.wrapPanel.Children.Clear();

            string[] filePaths = Directory.GetFiles(folderPath);
            for (int i = 0; i < filePaths.Length; ++i)
            {
                string file = filePaths[i];
                FileKinectButton tile = new FileKinectButton();
                tile.Label = i.ToString() + " " + System.IO.Path.GetFileName(file);
                tile.FileName = System.IO.Path.GetFileName(file);
                setTileAsNormal(tile);
                this.wrapPanel.Children.Add(tile);
            }

            this.wrapPanel.Height = ( (filePaths.Length + 2) / 3) * 220;
        }

        public void setTileAsNormal(KinectTileButton button)
        {
            button.Width = 350;
            button.Height = 200;
            button.Margin = new Thickness(5);
        }

        public void setTileAsSelected(KinectTileButton button)
        {
            button.Width = 350;
            button.Height = 200;
            button.Margin = new Thickness(5);
            button.Background = Brushes.DarkGray;
        }

        /// <summary>
        /// CLR Property Wrappers for PageLeftEnabledProperty
        /// </summary>
        public bool PageLeftEnabled
        {
            get
            {
                return (bool)GetValue(PageLeftEnabledProperty);
            }

            set
            {
                this.SetValue(PageLeftEnabledProperty, value);
            }
        }

        /// <summary>
        /// CLR Property Wrappers for PageRightEnabledProperty
        /// </summary>
        public bool PageRightEnabled
        {
            get
            {
                return (bool)GetValue(PageRightEnabledProperty);
            }

            set
            {
                this.SetValue(PageRightEnabledProperty, value);
            }
        }

        /// <summary>
        /// Called when the KinectSensorChooser gets a new sensor
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="args">event arguments</param>
        private static void SensorChooserOnKinectChanged(object sender, KinectChangedEventArgs args)
        {
            if (args.OldSensor != null)
            {
                try
                {
                    args.OldSensor.DepthStream.Range = DepthRange.Default;
                    args.OldSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    args.OldSensor.DepthStream.Disable();
                    args.OldSensor.SkeletonStream.Disable();
                }
                catch (InvalidOperationException)
                {
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                }
            }

            if (args.NewSensor != null)
            {
                try
                {
                    args.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                    args.NewSensor.SkeletonStream.Enable();

                    try
                    {
                        args.NewSensor.DepthStream.Range = DepthRange.Near;
                        args.NewSensor.SkeletonStream.EnableTrackingInNearRange = true;
                    }
                    catch (InvalidOperationException)
                    {
                        // Non Kinect for Windows devices do not support Near mode, so reset back to default mode.
                        args.NewSensor.DepthStream.Range = DepthRange.Default;
                        args.NewSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    }
                }
                catch (InvalidOperationException)
                {
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                }
            }
        }

        private void SendMessage(string message)
        {
            if (this.MessageSent != null)
            {
                this.MessageSent(message);
            }
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.sensorChooser.Stop();
        }

        /// <summary>
        /// Handle a button click from the wrap panel.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void KinectTileButtonClick(object sender, RoutedEventArgs e)
        {
            var button = (FileKinectButton)e.OriginalSource;
            //var selectionDisplay = new SelectionDisplay(button.FilePath as string);
            //this.kinectRegionGrid.Children.Add(selectionDisplay);
            
            clickButton(button);
            
            e.Handled = true;
        }

        private void clickButton(FileKinectButton button)
        {
            this.SendMessage("open:" + button.FileName);
        }

        /// <summary>
        /// Handle paging right (next button).
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void PageRightButtonClick(object sender, RoutedEventArgs e)
        {
            scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + PixelScrollByAmount);
        }

        /// <summary>
        /// Handle paging left (previous button).
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void PageLeftButtonClick(object sender, RoutedEventArgs e)
        {
            scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - PixelScrollByAmount);
        }

        /// <summary>
        /// Change button state depending on scroll viewer position
        /// </summary>
        private void UpdatePagingButtonState()
        {
            this.PageLeftEnabled = scrollViewer.HorizontalOffset > ScrollErrorMargin;
            this.PageRightEnabled = scrollViewer.HorizontalOffset < scrollViewer.ScrollableWidth - ScrollErrorMargin;
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}
