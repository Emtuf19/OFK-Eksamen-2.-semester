using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace _2_Semester_Eksamen.Model
{
    public class Practice
    {
        public int PracticeID { get; set; }
        public string PracticeName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        //Bliver brugt i PracticeListBox i PracticeWindow, for at vise antal tilmeldte medlemmer (selvom den ikke viser referencen)
        public int MemberCount => Members?.Count ?? 0;

        public List<Trainer> Trainers { get; set; } = new List<Trainer>();
        public List<Member> Members { get; set; } = new List<Member>();
    }
}
