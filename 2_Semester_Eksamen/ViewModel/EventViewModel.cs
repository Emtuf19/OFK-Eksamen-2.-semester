using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using _2_Semester_Eksamen.Commands;
using _2_Semester_Eksamen.Model;

namespace _2_Semester_Eksamen.ViewModel
{
    public class EventViewModel : ViewModelBase
    {
        public ObservableCollection<Event> Events { get; set; } = new();
        //public ObservableCollection<Event> SelectedEvents { get; set; } = new();

        public RelayCommand DeleteEventCommand { get; set; }
        public RelayCommand CreateEventCommand { get; set; }
        public RelayCommand UpdateEventCommand { get; set; }

        private Event _selectedEvent;
        public Event SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                _selectedEvent = value;
                OnPropertyChanged();

                if (_selectedEvent != null)
                {
                    EventName = _selectedEvent?.EventName;
                    Description = _selectedEvent?.Description;
                    AgeGroup = _selectedEvent?.AgeGroup;
                    Price = _selectedEvent?.Price ?? 0;
                    Time = _selectedEvent.Time;

                    DeleteEventCommand?.RaiseCanExecuteChanged();
                }
            }
        }

        private string _eventName;
        public string EventName
        {
            get => _eventName;
            set
            {
                _eventName = value;
                OnPropertyChanged(nameof(EventName));
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private string _ageGroup;
        public string AgeGroup
        {
            get => _ageGroup;
            set
            {
                _ageGroup = value;
                OnPropertyChanged(nameof(AgeGroup));
            }
        }

        private double _price;
        public double Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        private DateTime _time;
        public DateTime Time
        {
            get => _time;
            set
            {
                _time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        public EventViewModel()
        {
            LoadEvents();

            DeleteEventCommand = new RelayCommand(DeleteEvent, CanDeleteEvent);
            CreateEventCommand = new RelayCommand(CreateEvent, CanCreateEvent);
            UpdateEventCommand = new RelayCommand(UpdateEvent, CanUpdateEvent);
        }

        private void DeleteEvent()
        {
            if (SelectedEvent != null)
            {
                var repo = new EventRepository();
                repo.Delete(SelectedEvent.EventID);
                Events.Remove(SelectedEvent);
            }
            
            LoadEvents();
        }

        private bool CanDeleteEvent()
        {
            return SelectedEvent != null;
        }

        // Called by the View when the user confirmed creation in the dialog
        public void CreateEvent()
        {
            var newEvent = new Event
            {
                EventName = EventName,
                Description = Description,
                AgeGroup = AgeGroup,
                Price = Price,
                Time = Time
            };
            var repo = new EventRepository();
            repo.Add(newEvent);
            Events.Add(newEvent);

            LoadEvents();
        }

        private bool CanCreateEvent()
        {
            return !string.IsNullOrEmpty(EventName) && !string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(AgeGroup) && Price >= 0;
        }

        // Called by the View when the user confirmed update in the dialog
        public void UpdateEvent()
        {
            if (SelectedEvent != null)
            {
                SelectedEvent.EventName = EventName;
                SelectedEvent.Description = Description;
                SelectedEvent.AgeGroup = AgeGroup;
                SelectedEvent.Price = Price;
                SelectedEvent.Time = Time;

                var repo = new EventRepository();
                repo.Update(SelectedEvent);
            }

            LoadEvents();
        }


        private bool CanUpdateEvent()
        {
            return SelectedEvent != null && !string.IsNullOrEmpty(EventName) && !string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(AgeGroup) && Price >= 0;
        }

        private void LoadEvents()
        {
            Events.Clear();

            var repo = new EventRepository();
            var EventsFromDB = repo.GetAll();

            foreach (var ev in EventsFromDB)
            {
                Events.Add(ev);
            }
        }
    }
}
