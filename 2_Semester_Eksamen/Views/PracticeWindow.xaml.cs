using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using _2_Semester_Eksamen.ViewModel;
using _2_Semester_Eksamen.Model;

namespace _2_Semester_Eksamen.Views
{
    /// <summary>
    /// Interaction logic for PracticeWindow.xaml
    /// </summary>
    public partial class PracticeWindow : Page
    {
        public PracticeWindow()
        {
            InitializeComponent();
            DataContext = new CalendarViewModel();
        }
    }
}
