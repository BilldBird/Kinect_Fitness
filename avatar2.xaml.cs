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

namespace KinectHubDemo
{
    /// <summary>
    /// avatar2.xaml 的交互逻辑
    /// </summary>
    public partial class avatar2 : Page
    {
        private List<Button> buttons;
        public avatar2()
        {
            InitializeComponent();
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
            //HeadImage = 4;
            Application.Current.Properties["HeadImage"] = "4";
            NavigationService.Navigate(new Uri("name.xaml", UriKind.Relative));
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //HeadImage = 5;
            Application.Current.Properties["HeadImage"] = "5";
            NavigationService.Navigate(new Uri("name.xaml", UriKind.Relative));
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            //HeadImage = 6;
            Application.Current.Properties["HeadImage"] = "6";
            NavigationService.Navigate(new Uri("name.xaml", UriKind.Relative));
        }
    }
}
