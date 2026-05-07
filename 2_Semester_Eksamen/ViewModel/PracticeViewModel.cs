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
                    DeletePracticeCommand?.RaiseCanExecuteChanged();
                    EditPracticeCommand?.RaiseCanExecuteChanged();
                }
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
                LoadPractices();
            }
        }

        private bool _isPracticePopupOpen;
        public bool IsPracticePopupOpen
        {
            get => _isPracticePopupOpen;
            set
            {
                _isPracticePopupOpen = value;
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

        private Practice _editingPractice;
        public Practice EditingPractice
        {
            get => _editingPractice;
            set
            {
                _editingPractice = value;
                OnPropertyChanged();
            }
        }

        public List<int> Hours { get; } = Enumerable.Range(0, 24).ToList();
        public List<int> Minutes { get; } = new List<int> { 00, 15, 30, 45 };

        private int _startHour;
        public int StartHour
        {
            get => _startHour;
            set
            {
                _startHour = value;
                OnPropertyChanged();
            }
        }

        private int _startMinute;
        public int StartMinute
        {
            get => _startMinute;
            set
            {
                _startMinute = value;
                OnPropertyChanged();
            }
        }

        private int _endHour;
        public int EndHour
        {
            get => _endHour;
            set
            {
                _endHour = value;
                OnPropertyChanged();
            }
        }

        private int _endMinute;
        public int EndMinute
        {
            get => _endMinute;
            set
            {
                _endMinute = value;
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

        public RelayCommand DeletePracticeCommand { get; }
        public RelayCommand OpenCreatePracticeCommand { get; set; }
        public RelayCommand EditPracticeCommand { get; set; }
        public RelayCommand SavePracticeCommand { get; set; }
        public RelayCommand CancelEditPracticeCommand { get; }

        public RelayCommand CancelParticipationCommand { get; }

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

        private void ExecuteDeletePractice()
        {
            if (SelectedPractice == null)
                return;

            SelectedPractice.PracticeName = "Træning Aflyst";

            _practiceRepository.Update(SelectedPractice);
            LoadPractices();

        }

        public PracticeViewModel()
        {
            LoadPractices();

            DeletePracticeCommand = new RelayCommand(ExecuteDeletePractice, ()=> SelectedPractice != null);
            OpenCreatePracticeCommand = new RelayCommand(OpenCreatePractice);
            EditPracticeCommand = new RelayCommand(EditPractice, ()=> SelectedPractice != null);
            SavePracticeCommand = new RelayCommand(SavePractice);
            CancelEditPracticeCommand = new RelayCommand(CancelEdit);

            CancelParticipationCommand = new RelayCommand(ExecuteCancelParticipation, ()=> SelectedPractice != null && MemberIDInput > 0);
        }

        private void OpenCreatePractice()
        {
            EditingPractice = new Practice
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1)
            };

            DatePart = EditingPractice.StartTime.Date;

            StartHour = EditingPractice.StartTime.Hour;
            StartMinute = EditingPractice.StartTime.Minute;

            EndHour = EditingPractice.EndTime.Hour;
            EndMinute = EditingPractice.EndTime.Minute;

            IsEditMode = false;
            IsPracticePopupOpen = true;
        }

        private void EditPractice()
        {
            if (SelectedPractice == null)
                return;

            EditingPractice = new Practice
            {
                PracticeID = SelectedPractice.PracticeID,
                PracticeName = SelectedPractice.PracticeName,
                StartTime = SelectedPractice.StartTime,
                EndTime = SelectedPractice.EndTime
            };

            DatePart = EditingPractice.StartTime.Date;

            StartHour = EditingPractice.StartTime.Hour;
            StartMinute = EditingPractice.StartTime.Minute;

            EndHour = EditingPractice.EndTime.Hour;
            EndMinute = EditingPractice.EndTime.Minute;

            IsEditMode = true;
            IsPracticePopupOpen = true;
        }

        private void SavePractice()
        {
            if (EditingPractice == null)
                return;

            var start = EditingPractice.StartTime = DatePart.Date.AddHours(StartHour).AddMinutes(StartMinute);
            var end = EditingPractice.EndTime = DatePart.Date.AddHours(EndHour).AddMinutes(EndMinute);

            if (start >= end || start < DateTime.Now)
            {
                MessageBox.Show("Ugyldige tider!", "Fejl", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (IsEditMode)
            {
                SelectedPractice.PracticeName = EditingPractice.PracticeName;
                SelectedPractice.StartTime = start;
                SelectedPractice.EndTime = end;

                _practiceRepository.Update(EditingPractice);
            }
            else
            {
                EditingPractice.StartTime = start;
                EditingPractice.EndTime = end;

                _practiceRepository.Add(EditingPractice);
                Practices.Add(EditingPractice);
            }

            CancelEdit();
            LoadPractices();
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

        private void CancelEdit()
        {
            EditingPractice = null;
            IsPracticePopupOpen = false;
        }
    }
}
