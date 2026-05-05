using System;
using System.Collections.Generic;
using System.Text;

namespace _2_Semester_Eksamen.ViewModel
{
    
    public class MemberViewModel : INotifyPropertyChanged
    {
        public readonly Member _member;
        
        private MemberViewModel() : this(new Member()) { }
    
        public MemberViewModel(Member member)
        {
            _member = member ?? new Member();
        }

        public int MemberID
        {
            get => _member.MemberID;
            set
    {
                if(_member.MemberID !=value)
                {
                    _member.MemberID = value;
                    OnPropertyChanged(nameof(MemberID));
                }
            }
        }

        public string MemberFirstName
        {
            get => _member.MemberFirstName ?? string.Empty;
            set
            {
                if ((_member.MemberFirstName ?? string.Empty) != (value ?? string.Empty))
                {
                    _member.MemberFirstName = value ?? string.Empty;
                    OnPropertyChanged(nameof(MemberFirstName));
                    OnPropertyChanged(nameof(Fullname));
                }
            }
        }

        public string MemberLastName
        {
            get => _member.MemberLastName ?? string.Empty:
            set
            {
                if ((_member.MemberLastName ?? string.Empty) != (value ?? string.Empty))
                {
                    _member.MemberLastName = value ?? string.Empty;
                    OnPropertyChanged(nameof(MemberLastName));
                    OnPropertyChanged(nameof(Fullname));
                }
    }
}

        public string Fullname => $"{MemberFirstName} {MemberLastName}".Trim();

        public Member ToModel() => _member;

        public event PropertyChangedEventHandler? Propertychanged;
        protected void OnPropertyChanged(string propName) => Propertychanged?.Invoke(this, new PropertyChangedEventArgs(propName));

    }

}
