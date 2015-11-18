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

namespace KinectHubDemo
{
    /// <summary>
    /// confirm.xaml 的交互逻辑
    /// </summary>
    public partial class confirm : Page
    {
        private List<Button> buttons;
        public int NewUser;
        public confirm()
        {
            InitializeComponent();
            label_gender.Content = (string)Application.Current.Properties["sex"];
            label_username.Content = (string)Application.Current.Properties["name"];
            string HeadImage1 = (string)Application.Current.Properties["HeadImage"];
            int HeadImage = Convert.ToInt32(HeadImage1);
            if (HeadImage == 1)
                image1.Source = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/male1.jpg"));
            if (HeadImage == 2)
                image1.Source = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/male2.jpg"));
            if (HeadImage == 3)
                image1.Source = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/male3.jpg"));
            if (HeadImage == 4)
                image1.Source = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/female1.jpg"));
            if (HeadImage == 5)
                image1.Source = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/female2.jpg"));
            if (HeadImage == 6)
                image1.Source = new BitmapImage(new Uri("C:/Users/jun/Desktop/kinect/5.14/Fitness_new/image/female3.jpg"));

        }
        private void InitializeButtons()
        {
            buttons = new List<Button>
			    {
			        button1,
					
					
			    };
        }
        private void button11_Click(object sender, RoutedEventArgs e)
        {
            //store
            // Use Access Data Engine Jet
            string strConnection = "Provider=Microsoft.Jet.OleDb.4.0;";
            // Database file path
            strConnection += @"Data Source = C:\Users\jun\Desktop\kinect\5.14\Fitness_new\bin\Debug\User.mdb";
            // Create OleDeConnection object
            OleDbConnection objConnection = new OleDbConnection(strConnection);
            // Connection start
            objConnection.Open();
            // Check connection status
            if (objConnection.State == ConnectionState.Open)
            {
                MessageBox.Show("Access database connect successful");
            }
            else
            {
                MessageBox.Show("fail");
            }
            string Gender = (string)Application.Current.Properties["sex"];
            string HeadImage1 = (string)Application.Current.Properties["HeadImage"];
            string Name = (string)Application.Current.Properties["name"];
            int HeadImage = Convert.ToInt32(HeadImage1);
            // New SQL command
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
            // Insert new user information into database
            // ID, Name, General, Level, Head avater, Experience
            OleDbCommand cmd2 = new OleDbCommand();
            string sqlstr = "insert into  User_Info values (" + NewUser + ",'" + Name + "','" + Gender + "',0," + HeadImage + ",1)";
            cmd2.CommandText = sqlstr;
            cmd2.Connection = objConnection;
            cmd2.ExecuteNonQuery();
            /*
            // Check current user information
            string sqlstr = "select * from User_Info where ID = " + CurrentUser;
            cmd.CommandText = sqlstr;
            cmd.Connection = objConnection;
            // Read user information
            OleDbDataReader DataReader = cmd.ExecuteReader();
            if (DataReader.Read())
            {
                head_image = DataReader.GetInt32(4);
                exp = DataReader.GetInt32(5);
            }
            this.progressBar1.Value = exp;
             */
            // End connection
            objConnection.Close();
            Application.Current.Properties["current"] = NewUser;
        }

      


    }
}
