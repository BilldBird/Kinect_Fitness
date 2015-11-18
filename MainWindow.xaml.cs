using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Coding4Fun.Kinect.Wpf;
using Coding4Fun.Kinect.Wpf.Controls;
using Microsoft.Kinect;
using System.Data;
using System.Data.OleDb;
using System.Windows.Navigation;

namespace KinectHubDemo
{
    public partial class MainWindow : Window
    {
        KinectSensor kinect;
        
        private List<Button> buttons;
        private Button hoveredButton;
        public int CurrentUser = 1;
        private bool isWindowsClosing = false;
        public int head_image;
        public int exp, level;

        
        /// <summary>
        /// 启动Kinect设备，默认初始化选项，并注册AllFramesReady同步事件
        /// </summary>
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
                kinect.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(kinect_ColorFrameReady);
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

        void kinect_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame == null)
                {
                    return;
                }

                //屏幕右下角显示彩色摄像，使用Coding4Fun.Kinect.Wpf的扩展方法
                videoImage.Source = colorFrame.ToBitmapSource();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.progressBar1.Maximum = 10;
            //progressBar1.ValueChanged+=new RoutedPropertyChangedEventHandler<double>(progressBar1_ValueChanged);
            kinectButton.Click += new RoutedEventHandler(kinectButton_Clicked);
            Application.Current.Properties["current"] = CurrentUser;
        }


        private void InitializeButtons()
        {
            buttons = new List<Button>
			    {
			        button1,
					button2,
					button3,
					button4,
					button5,
					button6,
                    button7
			    };
        }

        /// <summary>
        /// 悬停选择按钮处理
        /// </summary>
        /// <param name="hand">当前移动的悬浮手型光标</param>
        /// <param name="buttons">图片按钮集合</param>
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
            if (isWindowsClosing || !Window.GetWindow(hand).IsActive)
                return false;


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

        /*
        private void promoteButtonClickEvent(string info)
        {
            listBoxHoverEvent.Items.Add(string.Format("{0} : {1}", info, DateTime.Now.ToString("t")));
        }
        */
        //update exp and level
        private void updateEvent(int Exp, int Level)
        {
            this.progressBar1.Value = Exp;
            label2.Content = Level;
        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Window1 win1 = new Window1();
            win1.updateEvent += new update(updateEvent);
            win1.ShowDialog();
            startKinect();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Window2 win2 = new Window2();
            win2.updateEvent += new update(updateEvent);
            win2.ShowDialog();
            startKinect();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Window3 win3 = new Window3();
            win3.updateEvent += new update(updateEvent);
            win3.ShowDialog();
            startKinect();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            Window4 win4 = new Window4();
            win4.updateEvent += new update(updateEvent);
            win4.ShowDialog();
            startKinect();
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            Window5 win5 = new Window5();
            win5.updateEvent += new update(updateEvent);
            win5.ShowDialog();
            startKinect();
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            Window6 win6 = new Window6();
            win6.updateEvent += new update(updateEvent);
            win6.ShowDialog();
            startKinect();
        }
        

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            //this.Hide();
            //promoteButtonClickEvent("Button 7 Clicked");
            Window7 win7 = new Window7();
            
            //this.Close();
            //win7.Owner = this;
            
           
            CurrentUser = Convert.ToInt32(Application.Current.Properties["current"]);
            // Use Access Data Engine Jet
            string strConnection = "Provider=Microsoft.Jet.OleDb.4.0;";
            // Database file path
            strConnection += @"Data Source = C:\Users\jun\Desktop\kinect\5.14\Fitness_new\bin\Debug\User.mdb";
            // Create OleDeConnection object
            OleDbConnection objConnection = new OleDbConnection(strConnection);
            // Connection start
            objConnection.Open();
            // New SQL command
            OleDbCommand cmd = new OleDbCommand();
            // Check current user information
            string sqlstr = "select * from User_Info where ID = " + CurrentUser;
            cmd.CommandText = sqlstr;
            cmd.Connection = objConnection;
            // Read user information
            OleDbDataReader DataReader = cmd.ExecuteReader();
            if (DataReader.Read())
            {
                level = DataReader.GetInt32(3);
                head_image = DataReader.GetInt32(4);
                exp = DataReader.GetInt32(5);
            }
            this.progressBar1.Value = exp;
            label2.Content = level;
            // End connection
            objConnection.Close();

            // Based on user information, show avatar
            ImageBrush brush1 = new ImageBrush();
            if (head_image == 1)
            {
                BitmapImage image = new BitmapImage(new Uri(@"image/female1.jpg", UriKind.Relative));
                brush1.ImageSource = image;
                button7.Background = brush1;
            }
            else if (head_image == 2)
            {
                BitmapImage image = new BitmapImage(new Uri(@"image/female2.jpg", UriKind.Relative));
                brush1.ImageSource = image;
                button7.Background = brush1;
            }
            else if (head_image == 3)
            {
                BitmapImage image = new BitmapImage(new Uri(@"image/female3.jpg", UriKind.Relative));
                brush1.ImageSource = image;
                button7.Background = brush1;
            }
            else if (head_image == 4)
            {
                BitmapImage image = new BitmapImage(new Uri(@"image/male1.jpg", UriKind.Relative));
                brush1.ImageSource = image;
                button7.Background = brush1;
            }
            else if (head_image == 5)
            {
                BitmapImage image = new BitmapImage(new Uri(@"image/male2.jpg", UriKind.Relative));
                brush1.ImageSource = image;
                button7.Background = brush1;
            }
            else if (head_image == 6)
            {
                BitmapImage image = new BitmapImage(new Uri(@"image/male3.jpg", UriKind.Relative));
                brush1.ImageSource = image;
                button7.Background = brush1;
            }
            win7.Show();
            startKinect();
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
            InitializeButtons();
            startKinect();

            // Use Access Data Engine Jet
            string strConnection = "Provider=Microsoft.Jet.OleDb.4.0;";
            // Database file path
            strConnection += @"Data Source = C:\Users\Ada\Desktop\kinect\5.9\Fitness_new\bin\Debug\User.mdb";
            //strConnection += Server.MapPath(@"User.mdb");
            // Create OleDeConnection object
            OleDbConnection objConnection = new OleDbConnection(strConnection);
            // Connection start
            objConnection.Open();
            // New SQL command
            OleDbCommand cmd = new OleDbCommand();
            // Check current user information
            string sqlstr = "select * from User_Info where ID = " + CurrentUser;
            cmd.CommandText = sqlstr;
            cmd.Connection = objConnection;
            // Read user information
            OleDbDataReader DataReader = cmd.ExecuteReader();
            if (DataReader.Read())
            {
                level = DataReader.GetInt32(3);
                head_image = DataReader.GetInt32(4);
                exp = DataReader.GetInt32(5);
            }
            this.progressBar1.Value = exp;
            label2.Content = level;
            // End connection
            objConnection.Close();

            // Based on user information, show avatar
            ImageBrush brush1 = new ImageBrush();
            if (head_image == 1)
            {
                BitmapImage image = new BitmapImage(new Uri(@"image/female1.jpg", UriKind.Relative));
                brush1.ImageSource = image;
                button7.Background = brush1;
            }
            else if (head_image == 2)
            {
                BitmapImage image = new BitmapImage(new Uri(@"image/female2.jpg", UriKind.Relative));
                brush1.ImageSource = image;
                button7.Background = brush1;
            }
            else if (head_image == 3)
            {
                BitmapImage image = new BitmapImage(new Uri(@"image/female3.jpg", UriKind.Relative));
                brush1.ImageSource = image;
                button7.Background = brush1;
            }
            else if (head_image == 4)
            {
                BitmapImage image = new BitmapImage(new Uri(@"image/male1.jpg", UriKind.Relative));
                brush1.ImageSource = image;
                button7.Background = brush1;
            }
            else if (head_image == 5)
            {
                BitmapImage image = new BitmapImage(new Uri(@"image/male2.jpg", UriKind.Relative));
                brush1.ImageSource = image;
                button7.Background = brush1;
            }
            else if (head_image == 6)
            {
                BitmapImage image = new BitmapImage(new Uri(@"image/male3.jpg", UriKind.Relative));
                brush1.ImageSource = image;
                button7.Background = brush1;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isWindowsClosing = true;
            stopKinect();
        }

       


        //public RoutedPropertyChangedEventHandler<double> progressBar1_ValueChanged { get; set; }
    }
}