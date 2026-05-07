using _2_Semester_Eksamen.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using _2_Semester_Eksamen.ViewModel;

namespace _2_Semester_Eksamen.Views
{
    public partial class HomeWindow : Page
    {
        public HomeWindow()
        {
            InitializeComponent();
            DataContext = new HomeViewModel();
        }
    }
}