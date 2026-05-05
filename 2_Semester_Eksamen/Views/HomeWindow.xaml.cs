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
        public ObservableCollection<Event> Events { get; set; }
      
        //Fejl Meddelelser
        public string? Error { get; set; }
        public string? ErrorEvent { get; set; }

        public HomeWindow()
        {
            InitializeComponent();
            // Pratice 
            Practices = new ObservableCollection<Practice>();

            var repoPractice = new PracticeRepository();
            var practicesFromDb = repoPractice.GetAll();

            // hvis db er tom
            if (practicesFromDb == null)
            {
                Error = "Ingen Trænning";
                TrainingError_txt.Visibility = Visibility.Visible;
            }
            // hvis db ikke er tom
            else
            {
                foreach (var practice in practicesFromDb)
                {
                    Practices.Add(practice);
                }
            }

            //Events 
            Events = new ObservableCollection<Event>();

            var repoEvent = new EventRepository();
            var eventsFromDb = repoEvent.GetAll();

            // hvis db er tom
            if (eventsFromDb == null)
            {
                ErrorEvent = "Ingen Events";
                EventError_txt.Visibility = Visibility.Visible;
            }
            // hvis db ikke er tom
            else
            {
                foreach (var ev in eventsFromDb)
                {
                    Events.Add(ev);
                }
            }
            DataContext = this;
        }
    }
}