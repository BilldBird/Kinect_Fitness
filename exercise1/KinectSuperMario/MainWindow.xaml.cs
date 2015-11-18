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
using Microsoft.Samples.Kinect.WpfViewers;
using System.IO;
using Coding4Fun.Kinect.Wpf;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;

namespace KinectProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        private KinectSensor kinect;
        public Skeleton[] frameSkeletons;
        private bool isWindowsClosing = false; //窗口是否正在关闭中
        const int MaxSkeletonTrackingCount = 6; //最多同时可以跟踪的用户数
        Skeleton[] allSkeletons = new Skeleton[MaxSkeletonTrackingCount];
        public bool flag1 = false, flag2 = false;
        public int count = 30;
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 创建 host 对象
            System.Windows.Forms.Integration.WindowsFormsHost host = new System.Windows.Forms.Integration.WindowsFormsHost();

            // 实例化 axShockwaveFlash1
            AxShockwaveFlashObjects.AxShockwaveFlash axShockwaveFlash1 = new AxShockwaveFlashObjects.AxShockwaveFlash();

            // 装载.axShockwaveFlash1
            host.Child = axShockwaveFlash1;

            // 将 host 对象嵌入FlashGrid
            this.FlashGrid.Children.Add(host);

            // 设置 .swf 文件相对路径
            string swfPath = System.Environment.CurrentDirectory;
            swfPath += @"\1.swf";
            axShockwaveFlash1.Movie = swfPath;

            label1.Visibility = System.Windows.Visibility.Hidden;

            startKinect();
            //kinectSensorChooser1.KinectSensorChanged += new DependencyPropertyChangedEventHandler(kinectSensorChooser1_KinectSensorChanged);
        }

		private void startKinect()
		{
			if (KinectSensor.KinectSensors.Count > 0)
			{
				kinect = KinectSensor.KinectSensors[0];
				if (kinect == null)
				{
                return;
				}
				//Get Depth Stream, Color Stream
				kinect.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
				kinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
				var parameters = new TransformSmoothParameters
				{
					Smoothing = 0.5f,
					Correction = 0.5f,
					Prediction = 0.5f,
					JitterRadius = 0.05f,
					MaxDeviationRadius = 0.04f
				};
				kinect.SkeletonStream.Enable(parameters);

				kinect.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinect_SkeletonFrameReady);

                kinectColorViewer1.Kinect = kinect;
                //kinectSkeletonViewer1.Kinect = kinect;
				kinect.Start();
				//OperationRecognizer();
			}
			else
			{
				MessageBox.Show("No kinect device found");
			}
		}

        private void stopKinect(KinectSensor sensor)
        {
            if (sensor != null)
            {
                if (sensor.IsRunning)
                {
                    //stop sensor 
                    sensor.Stop();

                    //关闭音频流，如果当前已打开的话
                    if (sensor.AudioSource != null)
                    {
                        sensor.AudioSource.Stop();
                    }
                }
            }
        }


        void kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            //骨骼跟踪状态提示
            label1.Visibility = System.Windows.Visibility.Hidden;

            if (isWindowsClosing)
            {
                return;
            }

            //Get a skeleton
            Skeleton s = getClosetSkeleton(e);

            if (s == null)
            {
                return;
            }

            if (s.TrackingState != SkeletonTrackingState.Tracked){
                
                return;
            }

            //If skeleton tracked, show label
            if (s.TrackingState == SkeletonTrackingState.Tracked)
                label1.Visibility = System.Windows.Visibility.Visible;

            exercise1(s);

                //映射键盘事件，玩超级玛丽
                //if(flag==true)
                   // playGame_keyboard(s);
            
                //if(flag==false)
                   // playGame_mouse(s);
        }

        void exercise1(Skeleton s)
        {
            SkeletonPoint head = s.Joints[JointType.Head].Position;
            SkeletonPoint leftHand = s.Joints[JointType.HandLeft].Position;
            SkeletonPoint rightHand = s.Joints[JointType.HandRight].Position;
            SkeletonPoint leftElbow = s.Joints[JointType.ElbowLeft].Position;
            SkeletonPoint rightElbow = s.Joints[JointType.ElbowRight].Position;
            SkeletonPoint leftLeg = s.Joints[JointType.AnkleLeft].Position;
            SkeletonPoint rightLeg = s.Joints[JointType.AnkleRight].Position;
            SkeletonPoint leftKnee = s.Joints[JointType.KneeLeft].Position;
            SkeletonPoint rightKnee = s.Joints[JointType.KneeRight].Position;

            if ((leftHand.Y > head.Y) && (leftKnee.Z < rightKnee.Z))
                flag1 = true;
            else if ((rightHand.Y > head.Y) && (rightKnee.Z < leftKnee.Z))
                flag2 = true;
            else if (flag1 == true && flag2 == true)
            {
                flag1 = false;
                flag2 = false;
                count--;
                label4.Content = count;
            }

        }

        void exercise2(Skeleton s)
        {
            SkeletonPoint head = s.Joints[JointType.Head].Position;
            SkeletonPoint leftHand = s.Joints[JointType.HandLeft].Position;
            SkeletonPoint rightHand = s.Joints[JointType.HandRight].Position;
            SkeletonPoint leftElbow = s.Joints[JointType.ElbowLeft].Position;
            SkeletonPoint rightElbow = s.Joints[JointType.ElbowRight].Position;
            SkeletonPoint leftLeg = s.Joints[JointType.AnkleLeft].Position;
            SkeletonPoint rightLeg = s.Joints[JointType.AnkleRight].Position;
            SkeletonPoint leftKnee = s.Joints[JointType.KneeLeft].Position;
            SkeletonPoint rightKnee = s.Joints[JointType.KneeRight].Position;

            if ((leftHand.X > rightHand.X) && (leftLeg.X < rightLeg.X))
                flag1 = true;
            else if(flag1==true)
                {
                    flag1 = false;
                    count--;
                    System.Console.Write("There is {0} left.", count);
                    Console.Beep();
                }
            

        }

        private double LHPreviousPosition = 1.5;
        private double RHPreviousPosition = 1.5;
        private double LLPreviousPosition = 0.5;
        private double RLPreviousPosition = 0.5;
        void exercise3(Skeleton s)
        {
            SkeletonPoint head = s.Joints[JointType.Head].Position;
            SkeletonPoint leftHand = s.Joints[JointType.HandLeft].Position;
            SkeletonPoint rightHand = s.Joints[JointType.HandRight].Position;
            SkeletonPoint leftElbow = s.Joints[JointType.ElbowLeft].Position;
            SkeletonPoint rightElbow = s.Joints[JointType.ElbowRight].Position;
            SkeletonPoint leftLeg = s.Joints[JointType.AnkleLeft].Position;
            SkeletonPoint rightLeg = s.Joints[JointType.AnkleRight].Position;
            SkeletonPoint leftKnee = s.Joints[JointType.KneeLeft].Position;
            SkeletonPoint rightKnee = s.Joints[JointType.KneeRight].Position;

                if ((leftHand.Y-LHPreviousPosition)>0.05 && (rightLeg.Y-RLPreviousPosition)>0.05)
                    flag1=true;
                LHPreviousPosition = leftHand.Y;
                RHPreviousPosition = rightHand.Y;
                LLPreviousPosition = leftLeg.Y;
                RLPreviousPosition = rightLeg.Y;

                if (flag1 == true && (rightHand.Y - RHPreviousPosition) > 0.05 && (leftLeg.Y - LLPreviousPosition) > 0.05)
                    flag2 = true;
                else if(flag1==true&&flag2==true)
                {
                    flag1 = false;
                    flag2 = false;
                    count--;
                    System.Console.Write("There is {0} left.", count);
                    Console.Beep();
                }
            
        }

        void exercise4(Skeleton s)
        {
            SkeletonPoint head = s.Joints[JointType.Head].Position;
            SkeletonPoint leftHand = s.Joints[JointType.HandLeft].Position;
            SkeletonPoint rightHand = s.Joints[JointType.HandRight].Position;
            SkeletonPoint leftElbow = s.Joints[JointType.ElbowLeft].Position;
            SkeletonPoint rightElbow = s.Joints[JointType.ElbowRight].Position;
            SkeletonPoint leftLeg = s.Joints[JointType.AnkleLeft].Position;
            SkeletonPoint rightLeg = s.Joints[JointType.AnkleRight].Position;
            SkeletonPoint leftKnee = s.Joints[JointType.KneeLeft].Position;
            SkeletonPoint rightKnee = s.Joints[JointType.KneeRight].Position;
            int mark = 0, count = 30;

            while (count > 0)
            {
                if ((rightHand.X - leftHand.X) < 0.2 && ((rightKnee.X - leftKnee.X) < (rightLeg.X - leftLeg.X)))
                    mark++;
                else if (mark == 1 && (rightHand.X - leftHand.X) > 1.0 && ((rightKnee.X - leftKnee.X) > (rightLeg.X - leftLeg.X)))
                    mark++;
                if (mark == 2)
                {
                    count--;
                    System.Console.Write("There is {0} left.", count);
                    Console.Beep();
                    mark = 0;
                }
            }
        }

        void exercise5(Skeleton s)
        {
            SkeletonPoint head = s.Joints[JointType.Head].Position;
            SkeletonPoint leftHand = s.Joints[JointType.HandLeft].Position;
            SkeletonPoint rightHand = s.Joints[JointType.HandRight].Position;
            SkeletonPoint leftElbow = s.Joints[JointType.ElbowLeft].Position;
            SkeletonPoint rightElbow = s.Joints[JointType.ElbowRight].Position;
            SkeletonPoint leftLeg = s.Joints[JointType.AnkleLeft].Position;
            SkeletonPoint rightLeg = s.Joints[JointType.AnkleRight].Position;
            SkeletonPoint leftKnee = s.Joints[JointType.KneeLeft].Position;
            SkeletonPoint rightKnee = s.Joints[JointType.KneeRight].Position;
            int mark = 0, count = 30;
            LLPreviousPosition = 0.5;
            RLPreviousPosition = 0.5;
            while (count > 0)
            {
                if ((rightHand.X - leftHand.X) < 0.2 && (rightLeg.Y - RLPreviousPosition) > 0.03 && (leftLeg.Y - LLPreviousPosition) > 0.03)
                    mark++;

                LLPreviousPosition = leftLeg.Y;
                RLPreviousPosition = rightLeg.Y;

                if (mark == 1 && (rightHand.X - leftHand.X) > 1.0 && (rightLeg.Y - RLPreviousPosition) > 0.03 && (leftLeg.Y - LLPreviousPosition) > 0.03)
                    mark++;

                LLPreviousPosition = leftLeg.Y;
                RLPreviousPosition = rightLeg.Y;
                if (mark == 2 && rightHand.Y > head.Y && leftHand.Y > head.Y && (rightLeg.Y - RLPreviousPosition) > 0.03 && (leftLeg.Y - LLPreviousPosition) > 0.03)
                    mark++;
                LLPreviousPosition = leftLeg.Y;
                RLPreviousPosition = rightLeg.Y;
                if(mark == 3)
                {
                    count--;
                    System.Console.Write("There is {0} left.", count);
                    Console.Beep();
                    mark = 0;
                }
            }
        }

        void exercise6(Skeleton s)
        {
            SkeletonPoint head = s.Joints[JointType.Head].Position;
            SkeletonPoint leftHand = s.Joints[JointType.HandLeft].Position;
            SkeletonPoint rightHand = s.Joints[JointType.HandRight].Position;
            SkeletonPoint leftElbow = s.Joints[JointType.ElbowLeft].Position;
            SkeletonPoint rightElbow = s.Joints[JointType.ElbowRight].Position;
            SkeletonPoint leftLeg = s.Joints[JointType.AnkleLeft].Position;
            SkeletonPoint rightLeg = s.Joints[JointType.AnkleRight].Position;
            SkeletonPoint leftKnee = s.Joints[JointType.KneeLeft].Position;
            SkeletonPoint rightKnee = s.Joints[JointType.KneeRight].Position;
            SkeletonPoint hipCenter = s.Joints[JointType.HipCenter].Position;
            SkeletonPoint hipLeft = s.Joints[JointType.HipLeft].Position;
            SkeletonPoint hipRight = s.Joints[JointType.HipRight].Position;

            int flag1 = 0, flag2 = 0;
            int count = 30;
            double LLP = leftLeg.Z, RLP = rightLeg.Z;
            while (count > 0)
            {
                if (leftHand.Y > hipCenter.Y && rightHand.Y > hipCenter.Y)
                    flag1++;
                if (flag1 == 1 && (rightHand.X - leftHand.X) > (hipRight.X - hipLeft.X))
                    flag1++;
                if (Math.Abs(leftLeg.Z - LLP) > 0.05 && Math.Abs(rightLeg.Z - RLP) > 0.05)
                    flag2++;
                if (Math.Abs(leftLeg.Z - LLP) > 0.05 && Math.Abs(rightLeg.Z - RLP) > 0.05)
                    flag2++;
                if (flag1 == 2 && flag2 == 2)
                {
                    count--;
                    System.Console.Write("There is {0} left.", count);
                    Console.Beep();
                    flag1 = 0; flag2 = 0;
                }
            }
        }

        Skeleton getClosetSkeleton(SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrameData = e.OpenSkeletonFrame())
            {
                if (skeletonFrameData == null)
                {
                    return null;
                }

                skeletonFrameData.CopySkeletonDataTo(allSkeletons);

                //Linq语法，查找离Kinect最近的、被跟踪的骨骼
                Skeleton closestSkeleton = (from s in allSkeletons
                                            where s.TrackingState == SkeletonTrackingState.Tracked &&
                                                  s.Joints[JointType.Head].TrackingState == JointTrackingState.Tracked
                                            select s).OrderBy(s => s.Joints[JointType.Head].Position.Z)
                                    .FirstOrDefault();

                return closestSkeleton;
            }
        }

        private void TrackHand2SimulateMouseMove(Joint hand)
        {
            if (hand.TrackingState != JointTrackingState.Tracked)
                return;

            //获得屏幕的宽度和高度
            int screenWidth = (int)SystemParameters.PrimaryScreenWidth;
            int screenHeight = (int)SystemParameters.PrimaryScreenHeight;

            //将部位“手”的骨骼坐标映射为屏幕坐标
            //float posX = joinCursorHand.ScaleTo(screenWidth, screenHeight).Position.X;
            //float posY = joinCursorHand.ScaleTo(screenWidth, screenHeight).Position.Y;

            //将部位“手”的骨骼坐标映射为屏幕坐标；手只需要在有限范围内移动即可覆盖整个屏幕区域
            float posX = hand.ScaleTo(screenWidth, screenHeight, 0.2f, 0.2f).Position.X;
            float posY = hand.ScaleTo(screenWidth, screenHeight, 0.2f, 0.2f).Position.Y;
                        
            Joint scaledCursorJoint = new Joint
            {
                TrackingState = JointTrackingState.Tracked,
                Position = new SkeletonPoint
                {
                    X = posX,
                    Y = posY,
                    Z = hand.Position.Z
                }
            };

            int x = Convert.ToInt32(scaledCursorJoint.Position.X);
            int y = Convert.ToInt32(scaledCursorJoint.Position.Y);

            //MouseToolkit.SetCursorPos(Convert.ToInt32(scaledCursorJoint.Position.X), 
            //    Convert.ToInt32(scaledCursorJoint.Position.Y));

            int mouseX = Convert.ToInt32(x * 65536 / screenWidth);
            int mouseY = Convert.ToInt32(y * 65536 / screenHeight);

            MouseToolkit.mouse_event(MouseToolkit.MouseEventFlag.Absolute | MouseToolkit.MouseEventFlag.Move,
                mouseX, mouseY, 0, 0);

        }



        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isWindowsClosing = true;
            stopKinect(kinectSensorChooser1.Kinect); 
        }




    }
}
