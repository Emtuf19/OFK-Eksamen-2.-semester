using _2_Semester_Eksamen.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;
using _2_Semester_Eksamen.Commands;

namespace _2_Semester_Eksamen.ViewModel
{
    public class CalendarViewModel : ViewModelBase
    {
        public ObservableCollection<Practice> Practices { get; set; } = new();

        public ObservableCollection<Practice> SelectedPractices { get; set; } = new();

        private Practice _selectedPractice;
        public Practice SelectedPractice
        {
            get => _selectedPractice;
            set
            {
                _selectedPractice = value;
                OnPropertyChanged();

                if (SelectedPractice != null)
                {
                    PracticeName = _selectedPractice.PracticeName;
                    StartTime = _selectedPractice.StartTime;
                    EndTime = _selectedPractice.EndTime;
                }

                DeletePracticeCommand?.RaiseCanExecuteChanged();
                UpdatePracticeCommand?.RaiseCanExecuteChanged();
            }
        }

        private string _practiceName;
        public string PracticeName
        {
            get { return _practiceName; }
            set
            {
                _practiceName = value;
                OnPropertyChanged();
            }
        }

        private DateTime _startTime;
        public DateTime StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
                OnPropertyChanged();
            }
        }

        private DateTime _endTime;
        public DateTime EndTime
        {
            get => _endTime;
            set
            {
                _endTime = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand DeletePracticeCommand { get; }
        public RelayCommand CreatePracticeCommand { get; }
        public RelayCommand UpdatePracticeCommand { get; }

        private bool CanExecuteDeletePractice()
        {
            return SelectedPractice != null;
        }

        private void ExecuteDeletePractice()
        {
            var repo = new PracticeRepository();

            repo.Delete(SelectedPractice.PracticeID);

            SelectedPractices.Remove(SelectedPractice);

            SelectedPractice = null;
        }

        private bool CanExecuteCreatePractice()
        {
            return SelectedDate != null;
        }

        private void ExecuteCreatePractice()
        {
            DateTime date = SelectedDate.Value;

            var newPractice = new Practice
            {
                PracticeName = "New Practice",
                StartTime = date.Date.AddHours(18), // Default to 9 AM on the selected date
                EndTime = date.Date.AddHours(20) // Default to 10 AM on the selected date

                //StartTime = SelectedDate.Value,
                //EndTime = SelectedDate.Value.AddHours(1)
            };

            var repo = new PracticeRepository();
            repo.Create(newPractice);

            Practices.Add(newPractice);
            UpdateSelectedPractices();
        }

        private bool CanExecuteUpdatePractice()
        {
            return SelectedPractice != null;
        }

        private void ExecuteUpdatePractice()
        {
            SelectedPractice.PracticeName = PracticeName;
            SelectedPractice.StartTime = StartTime;
            SelectedPractice.EndTime = EndTime;

            var repo = new PracticeRepository();
            repo.Update(SelectedPractice);

            // No need to update the collection as we are modifying the existing object?
            //OnPropertyChanged(nameof(SelectedPractices));

            //opdater listen over valgte praksisser for at reflektere ændringerne
            UpdateSelectedPractices();
        }

        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
                CreatePracticeCommand?.RaiseCanExecuteChanged();
                UpdateSelectedPractices();
            }
        }

        public CalendarViewModel()
        {
            LoadPractices();

            DeletePracticeCommand = new RelayCommand(ExecuteDeletePractice, CanExecuteDeletePractice);
            CreatePracticeCommand = new RelayCommand(ExecuteCreatePractice, CanExecuteCreatePractice);
            UpdatePracticeCommand = new RelayCommand(ExecuteUpdatePractice, CanExecuteUpdatePractice);
        }

        private void LoadPractices()
        {
            var repo = new PracticeRepository();
            var PracticesFromDB = repo.GetAll();

            foreach (var p in PracticesFromDB)
            {
                Practices.Add(p);
            }
        }

        private void UpdateSelectedPractices()
        {
            SelectedPractices.Clear();

            if (SelectedDate == null)
                return;

            foreach (var p in Practices.Where(p => p.StartTime.Date == SelectedDate.Value.Date))
            {
                SelectedPractices.Add(p);
            }
        }
    }
}
