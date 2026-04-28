using _2_Semester_Eksamen.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace _2_Semester_Eksamen.Views
{
    public partial class HomeWindow : Page
    {
        public ObservableCollection<Practice> Practices { get; set; }

        public Practice ThisPractice { get; set; }
        public string Error { get; set; }


        public HomeWindow()
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
            NavigationService.Navigate(new HomeWindow());
        }

        private void Practice_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PracticeWindow());
        }

        private void Event_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EventWindow());
        }

        private void OverviewMember_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OverviewMemberWindow());
        }

        private void AboutUs_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AboutUsWindow());
        }

        private void Staff_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StaffWindow());
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            //View Medlem Page
            NavigationService.Navigate(new LoginWindow());
        }

    }
}