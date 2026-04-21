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
    /// Interaction logic for Træner.xaml
    /// </summary>
    public partial class Træner : Page
    {
        public Træner()
        {
            InitializeComponent();
        }

        

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            //View Medlem Page
            NavigationService.Navigate(new Login());
        }
    }
}
