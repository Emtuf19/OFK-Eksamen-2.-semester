using _2_Semester_Eksamen.Commands;
using _2_Semester_Eksamen.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace _2_Semester_Eksamen.ViewModel
{
    public class EventViewModel : ViewModelBase
    {
        public ObservableCollection<Event> Events { get; set; } = new();

        public RelayCommand DeleteEventCommand { get; set; }
        public RelayCommand CreateEventCommand { get; set; }
        public RelayCommand EditEventCommand { get; set; }
        public RelayCommand SaveEventCommand { get; set; }
        public RelayCommand CancelEditEventCommand { get; set; }

        public RelayCommand SignUpEventCommand { get; set; }
        public RelayCommand CancelSignUpEventCommand { get; set; }

        private Event _selectedEvent;
        public Event SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                _selectedEvent = value;
                OnPropertyChanged();
                DeleteEventCommand?.RaiseCanExecuteChanged();
                EditEventCommand?.RaiseCanExecuteChanged();
            }
        }

        private int _memberIDInput;
        public int MemberIDInput
        {
            get => _memberIDInput;
            set
            {
                _memberIDInput = value;
                OnPropertyChanged();
                CancelSignUpEventCommand?.RaiseCanExecuteChanged();
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

        private bool _isEventPopupOpen;
        public bool IsEventPopupOpen
        {
            get => _isEventPopupOpen;
            set
            {
                _isEventPopupOpen = value;
                OnPropertyChanged();
            }
        }

        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                OnPropertyChanged();
            }
        }

        private Event _editingEvent;
        public Event EditingEvent
        {
            get => _editingEvent;
            set
            {
                _editingEvent = value;
                OnPropertyChanged();
            }
        }

        public List<int> Hours { get; } = Enumerable.Range(0, 24).ToList();
        public List<int> Minutes { get; } = new List<int> { 00, 15, 30, 45 };
        private int _selectedHour;
        public int SelectedHour
        {
            get => _selectedHour;
            set
            {
                _selectedHour = value;
                OnPropertyChanged();
            }
        }

        private int _selectedMinute;
        public int SelectedMinute
        {
            get => _selectedMinute;
            set
            {
                _selectedMinute = value;
                OnPropertyChanged();
            }
        }

        private DateTime _datePart;
        public DateTime DatePart
        {
            get => _datePart;
            set
            {
                _datePart = value;
                OnPropertyChanged();
            }
        }

        public EventViewModel()
        {
            LoadEvents();

            DeleteEventCommand = new RelayCommand(DeleteEvent, () => SelectedEvent != null);
            CreateEventCommand = new RelayCommand(OpenCreateEvent);
            EditEventCommand = new RelayCommand(EditEvent, () => SelectedEvent != null);
            SaveEventCommand = new RelayCommand(SaveEvent);
            CancelEditEventCommand = new RelayCommand(CancelEdit);

            CancelSignUpEventCommand = new RelayCommand(CancelSignUpEvent, () => SelectedEvent != null && MemberIDInput > 0);
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

        private void OpenCreateEvent()
        {
            EditingEvent = new Event
            {
                Time = DateTime.Now
            };

            DatePart = EditingEvent.Time.Date;
            SelectedHour = EditingEvent.Time.Hour;
            SelectedMinute = EditingEvent.Time.Minute;

            IsEditMode = false;
            IsEventPopupOpen = true;
        }

        private void EditEvent()
        {
            if (SelectedEvent == null)
                return;

            EditingEvent = new Event
            {
                EventID = SelectedEvent.EventID,
                EventName = SelectedEvent.EventName,
                Description = SelectedEvent.Description,
                AgeGroup = SelectedEvent.AgeGroup,
                Price = SelectedEvent.Price,
                Time = SelectedEvent.Time
            };

            DatePart = EditingEvent.Time.Date;
            SelectedHour = EditingEvent.Time.Hour;
            SelectedMinute = EditingEvent.Time.Minute;

            IsEditMode = true;
            IsEventPopupOpen = true;
        }

        public void SaveEvent()
        {
            if (EditingEvent == null)
                return;

            EditingEvent.Time = DatePart.AddHours(SelectedHour).AddMinutes(SelectedMinute);

            var repo = new EventRepository();

            if (IsEditMode)
            {
                SelectedEvent.EventName = EditingEvent.EventName;
                SelectedEvent.Description = EditingEvent.Description;
                SelectedEvent.AgeGroup = EditingEvent.AgeGroup;
                SelectedEvent.Price = EditingEvent.Price;
                SelectedEvent.Time = EditingEvent.Time;

                repo.Update(SelectedEvent);
            }
            else
            {
                repo.Add(EditingEvent);
                Events.Add(EditingEvent);
            }

            ClosePopup();
            LoadEvents();
        }

        private void ClosePopup()
        {
            IsEventPopupOpen = false;
            EditingEvent = null;
        }

        private void CancelEdit()
        {
            EditingEvent = null;
            IsEventPopupOpen = false;
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

        private void CancelSignUpEvent()
        {
            try
            {
                var repo = new EventRepository();
                repo.RemoveMemberFromEvent(SelectedEvent.EventID, MemberIDInput);

                bool memberExists = SelectedEvent.Members?.Any(m => m.MemberID == MemberIDInput) ?? false;

                if (!memberExists)
                {
                    MessageBox.Show($"Medlem {MemberIDInput} er ikke tilmeldt Begivenhed.", "Afmelding mislykket", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                MessageBox.Show($"Medlem {MemberIDInput} er afmeldt Begivenhed.", "Afmelding fuldført", MessageBoxButton.OK, MessageBoxImage.Information);

                var member = SelectedEvent.Members?.FirstOrDefault(m => m.MemberID == MemberIDInput);

                if (member != null)
                {
                    SelectedEvent.Members.Remove(member);
                }

                MemberIDInput = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kunne ikke afmelde medlem.\n" + ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
