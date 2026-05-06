using _2_Semester_Eksamen.Commands;
using _2_Semester_Eksamen.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;


namespace _2_Semester_Eksamen.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        public ObservableCollection<Practice> Practices { get; set; } = new();
        public ObservableCollection<Event> Events { get; set; } = new();

        public HomeViewModel()
        {
            LoadPractices();
            LoadEvents();
        }

        //Fejl Meddelelser
        private string? error;
        public string? Error
        {
            get => error;
            set
            {
                error = value;
                OnPropertyChanged();
            }
        }
        private string? errorEvent;
        public string? ErrorEvent
        {
            get => errorEvent;
            set
            {
                errorEvent = value;
                OnPropertyChanged();
            }
        }
        public void LoadPractices()
        {
            Practices.Clear();

            var repoPractice = new PracticeRepository();
            var practicesFromDb = repoPractice.GetAll();

            // hvis db er tom
            if (practicesFromDb == null || practicesFromDb.Count == 0)
            {
                Error = "Ingen Træning";
            }
            // hvis db ikke er tom
            else
            {
                foreach (var practice in practicesFromDb)
                {
                    Practices.Add(practice);
                }
            }
        }
        public void LoadEvents()
        {
            Events.Clear();
            //Events 

            var repoEvent = new EventRepository();
            var eventsFromDb = repoEvent.GetAll();

            // hvis db er tom
            if (eventsFromDb == null || eventsFromDb.Count == 0)
            {
               ErrorEvent = "Ingen Events";
            }
            // hvis db ikke er tom
            else
            {
                foreach (var ev in eventsFromDb)
                {
                    Events.Add(ev);
                }
            }
        }
    }
}
