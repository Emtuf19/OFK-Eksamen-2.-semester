using _2_Semester_Eksamen.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;

namespace _2_Semester_Eksamen.ViewModel
{
    public class CalendarViewModel : ViewModelBase
    {
        public ObservableCollection<Practice> Practices { get; set; } = new();

        public ObservableCollection<Practice> SelectedPractices { get; set; } = new();

        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
                UpdateSelectedPractices();
            }
        }

        public CalendarViewModel()
        {
            LoadPractices();
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
