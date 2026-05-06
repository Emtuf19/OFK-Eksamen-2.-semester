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
using System.Security.Policy;

namespace _2_Semester_Eksamen.ViewModel
{
    public class PracticeViewModel : ViewModelBase
    {
        public ObservableCollection<Practice> Practices { get; set; } = new();

        private PracticeRepository _practiceRepository = new PracticeRepository();

        private int _memberIDInput;
        public int MemberIDInput
        {
            get => _memberIDInput; 
            set
            {
                _memberIDInput = value;
                OnPropertyChanged();
                CancelParticipationCommand?.RaiseCanExecuteChanged();
            }
        }

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


                    DeletePracticeCommand?.RaiseCanExecuteChanged();
                    UpdatePracticeCommand?.RaiseCanExecuteChanged();
                }
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
                LoadPractices();
            }
        }

        public RelayCommand DeletePracticeCommand { get; }
        public RelayCommand CreatePracticeCommand { get; }
        public RelayCommand UpdatePracticeCommand { get; }
        public RelayCommand CancelParticipationCommand { get; }

        private bool CanExecuteCancelParticipation()
        {
            return SelectedPractice != null && MemberIDInput > 0;
        }

        private void ExecuteCancelParticipation()
        {
            try
            {
                _practiceRepository.RemoveMemberFromPractice(SelectedPractice.PracticeID, MemberIDInput);

                bool memberExists = SelectedPractice.Members?.Any(m => m.MemberID == MemberIDInput) ?? false;

                if (!memberExists)
                {
                    MessageBox.Show($"Medlem {MemberIDInput} er ikke tilmeldt træningen.", "Afmelding mislykket", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                MessageBox.Show($"Medlem {MemberIDInput} er afmeldt træningen.", "Afmelding fuldført", MessageBoxButton.OK, MessageBoxImage.Information);

                var member = SelectedPractice.Members?.FirstOrDefault(m => m.MemberID == MemberIDInput);

                if (member != null)
                {
                    SelectedPractice.Members.Remove(member);
                }

                MemberIDInput = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kunne ikke afmelde medlem.\n" + ex.Message, "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

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

            _practiceRepository.Update(SelectedPractice);

            LoadPractices();

        }

        private bool CanExecuteCreatePractice()
        {
            return SelectedDate != null && !string.IsNullOrWhiteSpace(PracticeName) 
                   && StartTime.TimeOfDay != null && EndTime.TimeOfDay != null;
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

            if (newPractice.StartTime >= newPractice.EndTime || newPractice.StartTime < DateTime.Now)
            {
                MessageBox.Show("Ugyldige tider!", "Ugyldige tider", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                _practiceRepository.Create(newPractice);
                Practices.Add(newPractice);
            }

            LoadPractices();

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

            if (SelectedPractice.StartTime >= SelectedPractice.EndTime || SelectedPractice.StartTime < DateTime.Now)
            {
                MessageBox.Show("Ugyldige tider!", "Ugyldige tider", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                _practiceRepository.Update(SelectedPractice);
            }
            LoadPractices();
        }

        public PracticeViewModel()
        {
            LoadPractices();

            // Sæt default værdier for StartTime og EndTime
            ClearInputFields();

            DeletePracticeCommand = new RelayCommand(ExecuteDeletePractice, CanExecuteDeletePractice);
            CreatePracticeCommand = new RelayCommand(ExecuteCreatePractice, CanExecuteCreatePractice);
            UpdatePracticeCommand = new RelayCommand(ExecuteUpdatePractice, CanExecuteUpdatePractice);
            CancelParticipationCommand = new RelayCommand(ExecuteCancelParticipation, CanExecuteCancelParticipation);
        }

        private void LoadPractices()
        {
            Practices.Clear();

            var PracticesFromDB = _practiceRepository.GetAll();

            if (SelectedDate == null)
                return;

            foreach (var p in PracticesFromDB.Where(p => p.StartTime.Date == SelectedDate.Value.Date))
            {
                Practices.Add(p);
            }
        }
    }
}
