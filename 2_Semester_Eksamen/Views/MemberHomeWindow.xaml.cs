using _2_Semester_Eksamen.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace _2_Semester_Eksamen.Views
{
    /// <summary>
    /// Interaction logic for MemberHomeWindow.xaml
    /// </summary>
    public partial class MemberHomeWindow : Page
    {
        public ObservableCollection<Practice> Practices { get; set; }

        public Practice ThisPractice { get; set; }
        public string Error { get; set; }

        public MemberHomeWindow()
        {
            InitializeComponent();
            try
            {
                Practices = new ObservableCollection<Practice>();

                var repo = new PracticeRepository();
                var practicesFromDb = repo.GetAll();


                foreach (var practice in practicesFromDb)
                {
                    Practices.Add(practice);
                }

                DataContext = this;
            }
            catch (Exception ex)
            {
                Error = "Ingen Trænning";
                DataContext = this;
            }
        }


        private void Home_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MemberHomeWindow());
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            //View Medlem Page
            NavigationService.Navigate(new LoginWindow());
        }

        private void Practice_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MemberPracticeWindow());
        }

        private void Event_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MemberEventWindow());
        }

        private void AboutUs_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AboutUsWindow());
        }

        private void Staff_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StaffWindow());
        }
    }
}
