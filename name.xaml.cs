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
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using System.IO;
using System.Data;
using System.Data.OleDb;

namespace KinectHubDemo
{
    /// <summary>
    /// name.xaml 的交互逻辑
    /// </summary>
    public partial class name : Page
    {
        private List<Button> buttons;
        public String Name;
        private KinectSensor kinect;
        // declare a Speech Recognition Engine
        private SpeechRecognitionEngine sre;
        int NewUser;
        public name()
        {
            InitializeComponent();

            ImageBrush brush1 = new ImageBrush();
            BitmapImage image = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/microphone.jpg", UriKind.Relative));
            brush1.ImageSource = image;
            button2.Background = brush1;


            // Use Access Data Engine Jet
            string strConnection = "Provider=Microsoft.Jet.OleDb.4.0;";
            // Database file path
            strConnection += @"Data Source = C:\Users\jun\Desktop\kinect\5.14\Fitness_new\bin\Debug\User.mdb";
            // Create OleDeConnection object
            OleDbConnection objConnection = new OleDbConnection(strConnection);
            // Connection start
            objConnection.Open();
            OleDbCommand cmd = new OleDbCommand();
            // Count the number of all users
            string SqlQuery = "select count(*) from User_Info";
            cmd.CommandText = SqlQuery;
            cmd.Connection = objConnection;
            // Read user information
            OleDbDataReader DataReader = cmd.ExecuteReader();
            if (DataReader.Read())
            {
                NewUser = DataReader.GetInt32(0);
            }
            NewUser++;
            objConnection.Close();
            string No = Convert.ToString(NewUser);
            string User = "User";
            Name = User + No;
            label1.Content = Name;
            Application.Current.Properties["name"] = Name;
        }
        private void InitializeButtons()
        {
            buttons = new List<Button>
			    {
			        //button1,
					button2,
					button3
			    };
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
                /*
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

                //kinectColorViewer1.Kinect = kinect;
                //kinectSkeletonViewer1.Kinect = kinect;*/
                kinect.Start();
                nameRecognizer();
            }
            else
            {
                MessageBox.Show("No kinect device found");
            }
        }
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


        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            //语音识别信心度超过70%
            if (e.Result.Confidence >= 0.7)
            {
                string name = e.Result.Text.ToLower();

                if (name == "alice")
                {
                    label1.Content = "Alice?";
                    Application.Current.Properties["name"] = "Alice";
                }
                else if (name == "john")
                {
                    label1.Content = "John?";
                    Application.Current.Properties["name"] = "John";
                }
                else if (name == "lily")
                {
                    label1.Content = "Lily?";
                    Application.Current.Properties["name"] = "Lily";
                }
                else if (name == "jake")
                {
                    label1.Content = "Jake?";
                    Application.Current.Properties["name"] = "Jake";
                }
                else if (name == "mary")
                {
                    label1.Content = "Mary?";
                    Application.Current.Properties["name"] = "Mary";
                }
            }
        }

        void sre_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void sre_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            //throw new NotImplementedException();
        }
        // write the name recognition grammar
        private void nameRecognizer()
        {
            MessageBox.Show("Start Voice Recognition");
            System.Threading.Thread.Sleep(1000);
            //Get the Audio Source
            KinectAudioSource source = kinect.AudioSource;
            source.EchoCancellationMode = EchoCancellationMode.None;
            source.AutomaticGainControlEnabled = false;

            RecognizerInfo ri = GetKinectRecognizer();
            if (ri == null)
            {
                MessageBox.Show("Can't find Kinect Speech Recognizer!");
                return;
            }

            sre = new SpeechRecognitionEngine(ri.Id);
            /****************************************************************
                * 
                * Use this code to create grammar programmatically rather than from
                * a grammar file.
                * 
                * var directions = new Choices();
                * directions.Add(new SemanticResultValue("forward", "FORWARD"));
                * directions.Add(new SemanticResultValue("forwards", "FORWARD"));
                * directions.Add(new SemanticResultValue("straight", "FORWARD"));
                * directions.Add(new SemanticResultValue("backward", "BACKWARD"));
                * directions.Add(new SemanticResultValue("backwards", "BACKWARD"));
                * directions.Add(new SemanticResultValue("back", "BACKWARD"));
                * directions.Add(new SemanticResultValue("turn left", "LEFT"));
                * directions.Add(new SemanticResultValue("turn right", "RIGHT"));
                *
                * var gb = new GrammarBuilder { Culture = ri.Culture };
                * gb.Append(directions);
                *
                * var g = new Grammar(gb);
                * 
                ****************************************************************/

            // Create grammar to recognize names
            var names = new Choices();
            names.Add("alice");
            names.Add("john");
            names.Add("lily");
            names.Add("jake");
            names.Add("mary");
            names.Add("lisa");
            names.Add("amy");
            names.Add("emily");
            names.Add("daniel");

            var gb = new GrammarBuilder { Culture = ri.Culture };
            gb.Append(names);

            // Load new grammar
            var g = new Grammar(gb);
            sre.LoadGrammar(g);

            // Three Case: Recognized, Hyppothesized and Recognize fail 
            sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);
            sre.SpeechHypothesized += new EventHandler<SpeechHypothesizedEventArgs>(sre_SpeechHypothesized);
            sre.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(sre_SpeechRecognitionRejected);

            // Initialize and start Kinect audio stream
            Stream s = source.Start();
            sre.SetInputToAudioStream(s, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));

            // Start recognition engine asynchronous, can recognize multiple time
            sre.RecognizeAsync(RecognizeMode.Multiple);

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

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            startKinect();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (label1.Content != "Label")
            {

                NavigationService.Navigate(new Uri("confirm.xaml", UriKind.Relative));
            }
        }
    }
}