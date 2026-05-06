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

namespace _2_Semester_Eksamen.Views
{
    /// <summary>
    /// Interaction logic for EventWindow.xaml
    /// </summary>
    public partial class EventWindow : Page
    {
        public EventWindow()
        {
            InitializeComponent();

            var mainWindow = (MainWindow)Application.Current.MainWindow;

            if (mainWindow.CurrentRole == "Trainer")
            {
                EventButtonsTrainer.Visibility = Visibility.Visible;
                EventButtonsMember.Visibility = Visibility.Collapsed;
            }
            else if (mainWindow.CurrentRole == "Member")
            {
                EventButtonsTrainer.Visibility = Visibility.Collapsed;
                EventButtonsMember.Visibility = Visibility.Visible;
            }
            DataContext = new EventViewModel();
        }
    }
}
