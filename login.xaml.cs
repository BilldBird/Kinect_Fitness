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

using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls.Primitives;
using Coding4Fun.Kinect.Wpf;
using Coding4Fun.Kinect.Wpf.Controls;
using Microsoft.Kinect;
using System.Data;
using System.Data.OleDb;

namespace KinectHubDemo
{
    /// <summary>
    /// login.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class login : Page
    {
        private List<Button> buttons;
        public int CurrentUser = 1;
        private bool isWindowsClosing = false;
        public int he;
        public int name;
        public int user1_head;
        public int user2_head;
        public int user3_head;
        public string user1_name, user2_name, user3_name;
        public int CurrentUserID;

        KinectSensor kinect;

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
                //kinect.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinect_SkeletonFrameReady);

                // Start Kinect device
                kinect.Start();
            }
            else
            {
                MessageBox.Show("No Kinect Device found.");
            }
        }

        public login()
        {
            InitializeComponent();
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
            string sqlstr = "select * from User_Info where ID = 1";
            cmd.CommandText = sqlstr;
            cmd.Connection = objConnection;
            // Read user information
            OleDbDataReader DataReader = cmd.ExecuteReader();
            if (DataReader.Read())
            {
                user1_name = DataReader.GetString(1);
                user1_head = DataReader.GetInt32(4);
            }
            // New SQL command
            OleDbCommand cmd2 = new OleDbCommand();
            // Check current user information
            string sqlstr2 = "select * from User_Info where ID = 2";
            cmd2.CommandText = sqlstr2;
            cmd2.Connection = objConnection;
            // Read user information
            OleDbDataReader DataReader2 = cmd2.ExecuteReader();
            if (DataReader2.Read())
            {
                user2_name = DataReader2.GetString(1);
                user2_head = DataReader2.GetInt32(4);
            }
            // New SQL command
            OleDbCommand cmd3 = new OleDbCommand();
            // Check current user information
            string sqlstr3 = "select * from User_Info where ID = 3";
            cmd3.CommandText = sqlstr3;
            cmd3.Connection = objConnection;
            // Read user information
            OleDbDataReader DataReader3 = cmd3.ExecuteReader();
            if (DataReader3.Read())
            {
                user3_name = DataReader3.GetString(1);
                user3_head = DataReader3.GetInt32(4);
            }
            // End connection
            objConnection.Close();

            ImageBrush add = new ImageBrush();
            BitmapImage image1 = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/adduser.jpg", UriKind.Relative));
            add.ImageSource = image1;
            adduser.Background = add;

            label1.Content = user1_name;
            label2.Content = user2_name;
            label3.Content = user3_name;
            ImageBrush brush1 = new ImageBrush();
            

            if (user1_head == 1)
            {
                BitmapImage image = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/male1.jpg", UriKind.Relative));
                brush1.ImageSource = image;
                user1.Background = brush1;
            }
            else if (user1_head == 2)
            {
                BitmapImage image = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/male2.jpg", UriKind.Relative));
                brush1.ImageSource = image;
                user1.Background = brush1;
            }
            else if (user1_head == 3)
            {
                BitmapImage image = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/male3.jpg", UriKind.Relative));
                brush1.ImageSource = image;
                user1.Background = brush1;
            }
            else if (user1_head == 4)
            {
                BitmapImage image = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/female1.jpg", UriKind.Relative));
                brush1.ImageSource = image;
                user1.Background = brush1;
            }
            else if (user1_head == 5)
            {
                BitmapImage image = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/female2.jpg", UriKind.Relative));
                brush1.ImageSource = image;
                user1.Background = brush1;
            }
            else if (user1_head == 6)
            {
                BitmapImage image = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/female3.jpg", UriKind.Relative));
                brush1.ImageSource = image;
                user1.Background = brush1;
            }

            ImageBrush brush2 = new ImageBrush();
            if (user2_head == 1)
            {
                BitmapImage image = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/male1.jpg", UriKind.Relative));
                brush2.ImageSource = image;
                user2.Background = brush2;
            }
            else if (user2_head == 2)
            {
                BitmapImage image = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/male2.jpg", UriKind.Relative));
                brush2.ImageSource = image;
                user2.Background = brush2;
            }
            else if (user2_head == 3)
            {
                BitmapImage image = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/male3.jpg", UriKind.Relative));
                brush2.ImageSource = image;
                user2.Background = brush2;
            }
            else if (user2_head == 4)
            {
                BitmapImage image = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/female1.jpg", UriKind.Relative));
                brush2.ImageSource = image;
                user2.Background = brush2;
            }
            else if (user2_head == 5)
            {
                BitmapImage image = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/female2.jpg", UriKind.Relative));
                brush2.ImageSource = image;
                user2.Background = brush2;
            }
            else if (user2_head == 6)
            {
                BitmapImage image = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/female3.jpg", UriKind.Relative));
                brush2.ImageSource = image;
                user2.Background = brush2;
            }

            ImageBrush brush3 = new ImageBrush();
            if (user3_head == 1)
            {
                BitmapImage image = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/male1.jpg", UriKind.Relative));
                brush3.ImageSource = image;
                user3.Background = brush3;
            }
            else if (user3_head == 2)
            {
                BitmapImage image = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/male2.jpg", UriKind.Relative));
                brush3.ImageSource = image;
                user3.Background = brush3;
            }
            else if (user3_head == 3)
            {
                BitmapImage image = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/male3.jpg", UriKind.Relative));
                brush3.ImageSource = image;
                user3.Background = brush3;
            }
            else if (user3_head == 4)
            {
                BitmapImage image = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/female1.jpg", UriKind.Relative));
                brush3.ImageSource = image;
                user3.Background = brush3;
            }
            else if (user3_head == 5)
            {
                BitmapImage image = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/female2.jpg", UriKind.Relative));
                brush3.ImageSource = image;
                user3.Background = brush3;
            }
            else if (user3_head == 6)
            {
                BitmapImage image = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/female3.jpg", UriKind.Relative));
                brush3.ImageSource = image;
                user3.Background = brush3;
            }
            
        }


        private void InitializeButtons()
        {
            buttons = new List<Button>
			    {
			        user1,
					user2,
					user3,
					adduser,
					
			    };
        }

        private void user1_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties["current"] = "1";
            
            
        }

        private void user2_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties["current"] = "2";
        }

        private void user3_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties["current"] = "3";
        }

        private void adduser_Click(object sender, RoutedEventArgs e)
        {
            stopKinect();
            NavigationService.Navigate(new Uri("UI1.xaml", UriKind.Relative));
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
            

            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isWindowsClosing = true;
            //stopKinect();
        }

    }
}
