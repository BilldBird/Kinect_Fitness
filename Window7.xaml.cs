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
using System.Windows.Shapes;

using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls.Primitives;
using Coding4Fun.Kinect.Wpf;
using Coding4Fun.Kinect.Wpf.Controls;
using Microsoft.Kinect;
using System.Data;
using System.Data.OleDb;
using KinectHubControl;
using System.Windows.Navigation;
using System.Net;
using System.Windows.Media.Animation; 


namespace KinectHubDemo
{
    /// <summary>
    /// Interaction logic for Window7.xaml
    /// </summary>
    /// 
    //
    public delegate void ChangeTextHandler();

    public partial class Window7 : Window
    {
       
        KinectSensor kinect;
        private List<Button> buttons;
        private Button hoveredButton;
        private bool isWindowsClosing = false;

        public event ChangeTextHandler ChangeTextEvent;


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

        public bool IsButtonOverObject(FrameworkElement hand, List<Button> buttons)
        {
            // if (isWindowsClosing || !Window.GetWindow(hand).IsActive)
            //    return false;


            // 找到悬浮手型控件的中心点位置
            var handTopLeft = new Point(Canvas.GetTop(hand), Canvas.GetLeft(hand));
            double handLeft = handTopLeft.X + (hand.ActualWidth / 2);
            double handTop = handTopLeft.Y + (hand.ActualHeight / 2);
            /*
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
            }*/
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //InitializeButtons();
            startKinect();
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isWindowsClosing = true;
            //ChangeEvent();
            //stopKinect();
        }

        private void ChangeEvent()
        {
            string str = (string)Application.Current.Properties["current"];
            if (str != null) 
            {
                ChangeTextEvent();
            }
        }

        public Window7()
        {
            
            InitializeComponent();
            
            //kinectButton.Click += new RoutedEventHandler(kinectButton_Clicked);
            /*
            string strConnection = "Provider=Microsoft.Jet.OleDb.4.0;";
            strConnection += @"Data Source = C:\Users\jun\Desktop\kinect\4.28\Fitness_new\bin\Debug\User.mdb";

            OleDbConnection objConnection = new OleDbConnection(strConnection);
            objConnection.Open();
            if (objConnection.State == ConnectionState.Open)
            {
              //  MessageBox.Show("Access database connect successful");
            }
            else
            {
                MessageBox.Show("fail");
            }
            /*
            OleDbCommand cmd = new OleDbCommand();
            string sqlstr = "insert into  User_Info values (5,'Jake','M',1,3,0)";
            cmd.CommandText = sqlstr;
            cmd.Connection = objConnection;
            cmd.ExecuteNonQuery();
            */
            //objConnection.Close();
            
            //startKinect();
            //UI1 ui1 = new UI1();
            //frame1.Navigate(ui1);

            //get the current state
            
           
                stopKinect();
                logout logout = new logout();
                frame1.Navigate(logout);
              


        }
        private void InitializeButtons()
        {
            buttons = new List<Button>
			    {
			      
					button12
					
			    };
        }

        
        
        private void button12_Click(object sender, RoutedEventArgs e)
        {
                this.Close();
                
        }

      
        //public HoverButton kinectButton { get; set; }

        //public List<Button> buttons { get; set; }
    }
}
