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
using System.Data;
using System.Data.OleDb;
using Microsoft.Kinect;
using Coding4Fun.Kinect.Wpf;
using Coding4Fun.Kinect.Wpf.Controls;

using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls.Primitives;



namespace KinectHubDemo
{
    /// <summary>
    /// avatar1.xaml 的交互逻辑
    /// </summary>
    public partial class avatar1 : Page
    {
        private List<Button> buttons;
        KinectSensor kinect;
        private Button hoveredButton;
        private bool isWindowsClosing = false;

        private void startKinect()
        {
            if (KinectSensor.KinectSensors.Count > 0)
            {
                // Choose the first Kinect device
                kinect = KinectSensor.KinectSensors[0];
                if (kinect == null)
                    return;

                kinect.ColorStream.Enable();

                var tsp = new TransformSmoothParameters
                {
                    Smoothing = 0.5f,
                    Correction = 0.5f,
                    Prediction = 0.5f,
                    JitterRadius = 0.05f,
                    MaxDeviationRadius = 0.04f
                };
                kinect.SkeletonStream.Enable(tsp);

                // Start skeleton tracking
                //kinect.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(kinect_ColorFrameReady);
                kinect.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinect_SkeletonFrameReady);

                // Start Kinect device
                kinect.Start();
            }
            else
            {
                MessageBox.Show("No Kinect Device found.");
            }
        }

        void kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame frame = e.OpenSkeletonFrame())
            {

                if (frame == null)
                    return;

                if (frame.SkeletonArrayLength == 0)
                    return;

                Skeleton[] allSkeletons = new Skeleton[frame.SkeletonArrayLength];
                frame.CopySkeletonDataTo(allSkeletons);

                // Use Linq to select the closest and tracked skeleton, depending on the Z-axis position of head
                Skeleton closestSkeleton = (from s in allSkeletons
                                            where s.TrackingState == SkeletonTrackingState.Tracked &&
                                                  s.Joints[JointType.Head].TrackingState == JointTrackingState.Tracked
                                            select s).OrderBy(s => s.Joints[JointType.Head].Position.Z)
                                    .FirstOrDefault();

                if (closestSkeleton == null)
                    return;
                if (closestSkeleton.TrackingState != SkeletonTrackingState.Tracked)
                    return;

                var joints = closestSkeleton.Joints;

                Joint rightHand = joints[JointType.HandRight];
                Joint leftHand = joints[JointType.HandLeft];

                // depend on the Y-axis position, we check whether user use left hand or right hand
                var hand = (rightHand.Position.Y > leftHand.Position.Y)
                                ? rightHand
                                : leftHand;

                if (hand.TrackingState != JointTrackingState.Tracked)
                    return;

                // Get the screen width and height
                int screenWidth = (int)SystemParameters.PrimaryScreenWidth;
                int screenHeight = (int)SystemParameters.PrimaryScreenHeight;

                // Map the hand position into the screen
                float posX = hand.ScaleTo(screenWidth, screenHeight, 0.2f, 0.2f).Position.X;
                float posY = hand.ScaleTo(screenWidth, screenHeight, 0.2f, 0.2f).Position.Y;

                // If hand hover on button, trigger the click event
                OnButtonLocationChanged(kinectButton, buttons, (int)posX, (int)posY);
            }
        }

        /// <param name="X">SkeletonHandX</param>
        /// <param name="Y">SkeletonHandY</param>
        private void OnButtonLocationChanged(HoverButton hand, List<Button> buttons, int X, int Y)
        {
            if (IsButtonOverObject(hand, buttons))
                hand.Hovering(); // Mouse left button click
            else
                hand.Release();

            // Move hand cursor
            Canvas.SetLeft(hand, X - (hand.ActualWidth / 2));
            Canvas.SetTop(hand, Y - (hand.ActualHeight / 2));
        }


        private void kinectButton_Clicked(object sender, RoutedEventArgs e)
        {
            hoveredButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent, hoveredButton));
        }

        private bool IsButtonOverObject(FrameworkElement hand, List<Button> buttons)
        {
             //if (isWindowsClosing || !Window.GetWindow(hand).IsActive)
            //    return false;


            // 找到悬浮手型控件的中心点位置
            var handTopLeft = new Point(Canvas.GetTop(hand), Canvas.GetLeft(hand));
            double handLeft = handTopLeft.X + (hand.ActualWidth / 2);
            double handTop = handTopLeft.Y + (hand.ActualHeight / 2);

            //遍历图片按钮，判断Hand图标是否悬浮在其中之一
            foreach (Button target in buttons)
            {
                Point targetTopLeft = target.PointToScreen(new Point());
                if (handTop > targetTopLeft.X
                    && handTop < targetTopLeft.X + target.ActualWidth
                    && handLeft > targetTopLeft.Y
                    && handLeft < targetTopLeft.Y + target.ActualHeight)
                {
                    hoveredButton = target;
                    return true;
                }
            }
            return false;
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void stopKinect()
        {
            if (kinect != null)
            {
                if (kinect.Status == KinectStatus.Connected)
                {
                    //关闭Kinect设备
                    kinect.Stop();
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeButtons();
           // startKinect();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //isWindowsClosing = true;
            stopKinect();
        }

        public avatar1()
        {
            InitializeComponent();
            startKinect();
            kinectButton.Click += new RoutedEventHandler(kinectButton_Clicked);
     
        }
        private void InitializeButtons()
        {
            buttons = new List<Button>
			    {
			        button1,
					button2,
					button3
			    };
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //HeadImage = 1;
            stopKinect();
            Application.Current.Properties["HeadImage"] = "1";
            NavigationService.Navigate(new Uri("name.xaml", UriKind.Relative));
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //HeadImage = 2;
            stopKinect();
            Application.Current.Properties["HeadImage"] = "2";
            NavigationService.Navigate(new Uri("name.xaml", UriKind.Relative));
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            //HeadImage = 3;
            stopKinect();
            Application.Current.Properties["HeadImage"] = "3";
            NavigationService.Navigate(new Uri("name.xaml", UriKind.Relative));
        }
    }
}
