using _2_Semester_Eksamen.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace _2_Semester_Eksamen.Views
{
    public partial class Træner : Page
    {
        public ObservableCollection<Practice> Practices { get; set; }

        public Practice ThisPractice { get; set; }


        public Træner()
        {
            InitializeComponent();

            Practices = new ObservableCollection<Practice>();

            var repo = new PracticeRepository();
            var practicesFromDb = repo.GetAll();

            foreach (var practice in practicesFromDb)
            {
                Practices.Add(practice);
            }

            DataContext = this;
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            //View Medlem Page
            NavigationService.Navigate(new Login());
        }
    }
}