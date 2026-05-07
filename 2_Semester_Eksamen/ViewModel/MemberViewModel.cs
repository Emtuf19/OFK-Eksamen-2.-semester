using System;
using System.Collections.Generic;
using System.Text;
using _2_Semester_Eksamen.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using _2_Semester_Eksamen.Commands;
using System.Net.Mail;

namespace _2_Semester_Eksamen.ViewModel
{
    
    public class MemberViewModel : ViewModelBase
    {
        private MemberRepository _repo = new MemberRepository();

        public ObservableCollection<Member> Members { get; } = new();

        private Member _selectedMember;
        public Member SelectedMember
        {
            get => _selectedMember;
            set
            {
                _selectedMember = value;
                OnPropertyChanged();

                UpdateMemberCommand.RaiseCanExecuteChanged();
                DeleteMemberCommand.RaiseCanExecuteChanged();

                if (value == null)
                {
                    SelectedContact = new ContactInfo();
                    return;
                }

                if (value.ContactPersons == null || value.ContactPersons.Count == 0)
                {
                    var newContact = new ContactInfo { MemberID = value.MemberID };
                    value.ContactPersons.Add(newContact);
                }

                SelectedContact = value.ContactPersons[0];
            }
        }

        private ContactInfo _selectedContact = new ContactInfo();
        public ContactInfo SelectedContact
        {
            get => _selectedContact;
            set
            {
                _selectedContact = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AddMemberCommand { get; }
        public RelayCommand UpdateMemberCommand { get; }
        public RelayCommand DeleteMemberCommand { get; }

        public MemberViewModel()
        {
            Load();
            AddMemberCommand = new RelayCommand(ExecuteAdd);
            UpdateMemberCommand = new RelayCommand(ExecuteUpdate, () => SelectedMember != null);
            DeleteMemberCommand = new RelayCommand(ExecuteDelete, () => SelectedMember != null);
        }

        private void Load()
        {
            Members.Clear();
            foreach (var m in _repo.GetAll())
                Members.Add(m);
        }

        private void ExecuteAdd()
        {
            var newMember = new Member { MemberFirstName = "New", MemberLastName = "Member" };
            _repo.Add(newMember);
            Members.Add(newMember);
            SelectedMember = newMember;
            Load();
        }

        private void ExecuteUpdate()
        {
            if (SelectedMember == null) return;

                var id = SelectedMember.MemberID;

            if (!IsValidEmail(SelectedContact.ContactEmail))
            {
                MessageBox.Show("Angiv gyldig mail");
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedContact.ContactFirstName) || string.IsNullOrWhiteSpace(SelectedContact.ContactLastName))
            {
                MessageBox.Show("Angiv både For- og efternavn.");
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedContact.ContactPhoneNumber) || SelectedContact.ContactPhoneNumber.Length != 8)
            {
                MessageBox.Show("Angiv gyldigt telefonnummer");
                return;
            }

                _repo.Update(SelectedMember);

                Load();
                SelectedMember = null;
                SelectedMember = Members.FirstOrDefault(m => m.MemberID == id);
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void ExecuteDelete()
        {
            if (SelectedMember == null) return;
            _repo.Delete(SelectedMember.MemberID);
            Members.Remove(SelectedMember);
            SelectedMember = null;
            Load();
        }
    }

}
