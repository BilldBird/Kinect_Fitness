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
    /// logout.xaml 的交互逻辑
    /// </summary>
    public partial class logout : Page
    {
        private List<Button> buttons;
        public int CurrentUser = 1;
        private bool isWindowsClosing = false;
        public int he;
        public int name;
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

        public logout()
        {
            InitializeComponent();
        }

        private void InitializeButtons()
        {
            buttons = new List<Button>
			    {
			        button1,
					//button2
			    };
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            stopKinect();
            NavigationService.Navigate(new Uri("login.xaml", UriKind.Relative));
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //cancle
            //Application.Current.Properties["close"] = "1";
            
            this.Visibility = Visibility.Hidden;
            NavigationService.Navigate(new Uri("Window7.xaml", UriKind.Relative));
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
            startKinect();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isWindowsClosing = true;
            stopKinect();

        }




    }
}
