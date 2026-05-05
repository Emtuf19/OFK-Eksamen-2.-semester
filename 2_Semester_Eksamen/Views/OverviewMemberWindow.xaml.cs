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
using _2_Semester_Eksamen.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using _2_Semester_Eksamen.ViewModel;

namespace _2_Semester_Eksamen.Views
{
    public partial class OverviewMemberWindow : Page
    {
        public OverviewMemberWindow()
        {
            InitializeComponent();

            DataContext = new MemberViewModel();
        }
    }
}
