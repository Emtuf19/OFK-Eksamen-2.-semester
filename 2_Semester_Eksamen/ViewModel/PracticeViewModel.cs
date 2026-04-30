using _2_Semester_Eksamen.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;
using _2_Semester_Eksamen.Commands;
using System.Windows;

namespace _2_Semester_Eksamen.ViewModel
{
    public class PracticeViewModel : ViewModelBase
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
                CreatePracticeCommand?.RaiseCanExecuteChanged();
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
                CreatePracticeCommand?.RaiseCanExecuteChanged();
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
                CreatePracticeCommand?.RaiseCanExecuteChanged();
            }
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

        public RelayCommand DeletePracticeCommand { get; }
        public RelayCommand CreatePracticeCommand { get; }
        public RelayCommand UpdatePracticeCommand { get; }

        private bool CanExecuteDeletePractice()
        {
            return SelectedPractice != null;
        }

        private void ExecuteDeletePractice()
        {

            PracticeName = "Træning Aflyst";
            SelectedPractice.PracticeName = PracticeName;

            var date = SelectedPractice.StartTime.Date; // Behold den oprindelige dato
            SelectedPractice.StartTime = date.Add(StartTime.TimeOfDay);
            SelectedPractice.EndTime = date.Add(EndTime.TimeOfDay);
            
            var repo = new PracticeRepository();
            
            repo.Update(SelectedPractice);
            
            UpdateSelectedPractices();

        }

        private bool CanExecuteCreatePractice()
        {
            return SelectedDate != null && !string.IsNullOrWhiteSpace(PracticeName) 
                   && StartTime.TimeOfDay < EndTime.TimeOfDay;
        }

        private void ExecuteCreatePractice()
        {
            DateTime date = SelectedDate!.Value;

            var newPractice = new Practice
            {
                PracticeName = PracticeName,
                StartTime = date.Date.Add(StartTime.TimeOfDay), // Combine selected date with the time from StartTime
                EndTime = date.Date.Add(EndTime.TimeOfDay) // Combine selected date with the time from EndTime
            };

            var repo = new PracticeRepository();
            repo.Create(newPractice);

            Practices.Add(newPractice);
            UpdateSelectedPractices();

            ClearInputFields();
        }

        private void ClearInputFields()
        {
            PracticeName = string.Empty;
            StartTime = DateTime.Now;
            EndTime = DateTime.Now.AddHours(1);
        }

        private bool CanExecuteUpdatePractice()
        {
            return SelectedPractice != null;
        }

        private void ExecuteUpdatePractice()
        {
            SelectedPractice.PracticeName = PracticeName;

            var date = SelectedPractice.StartTime.Date; // Behold den oprindelige dato
            SelectedPractice.StartTime = date.Add(StartTime.TimeOfDay);
            SelectedPractice.EndTime = date.Add(EndTime.TimeOfDay);

            var repo = new PracticeRepository();
            if (SelectedPractice.StartTime >= SelectedPractice.EndTime)
            {
                MessageBox.Show("Ugyldige tider!", "Ugyldige tider", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                repo.Update(SelectedPractice);
            }
            UpdateSelectedPractices();
        }

        public PracticeViewModel()
        {
            LoadPractices();

            // Sæt default værdier for StartTime og EndTime
            ClearInputFields();

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
