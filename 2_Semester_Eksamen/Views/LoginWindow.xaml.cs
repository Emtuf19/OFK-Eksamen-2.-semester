using System;
using System.Collections.Generic;
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

namespace _2_Semester_Eksamen.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginWindow : Page
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void TrainerButton_Click(object sender, RoutedEventArgs e)
        {
            // View Home Page
            NavigationService.Navigate(new HomeWindow());
        }

        private void MemberButton_Click(object sender, RoutedEventArgs e)
        {
            //View Member Page
            NavigationService.Navigate(new MemberHomeWindow());
        }
    }
}
