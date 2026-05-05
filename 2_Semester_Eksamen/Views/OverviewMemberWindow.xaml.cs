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



namespace _2_Semester_Eksamen.Views
{
    /// Interaktion logik for OverviewMemberWindow.xaml

    public partial class OverviewMemberWindow : Page, INotifyPropertyChanged
    {

        private Member? _selecetMember;

        public ObservableCollection<Member> Members { get; } = new ObservableCollection<Member>();

        public Member? SelectedMember
        {
            get => _selecetMember;
            set
            {
                if(_selecetMember != value)
                {
                    _selecetMember = value;
                    OnPropertyChanged(nameof(SelectedMember));
                }
            }
        }



        public OverviewMemberWindow()
        {
            InitializeComponent();
            
            DataContext = this;
            LoadMembers();
        }

        private void LoadMembers()
        {
            try
            {
                var repo = new MemberRepository();
                var list = repo.GetAll();
                Members.Clear();

                foreach (var m in list)
                    Members.Add(m);

                if (Members.Count > 0)
                    SelectedMember = Members[0];
        }

            catch (Exception ex)
            {
                MessageBox.Show($"kunne ikke vise Medlemer; {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddMemberButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditMemberButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteMemberButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SafeMemberButton_Click(object sender, RoutedEventArgs e)
        {

        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
