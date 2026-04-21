using System;
using System.Collections.Generic;
using System.Text;

namespace _2_Semester_Eksamen.Model
{
    public class Practice
    {
        public int PracticeID { get; set; }
        public string PracticeName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public List<Trainer> Trainers { get; set; } = new List<Trainer>();
        public List<Member> Members { get; set; } = new List<Member>();
    }
}
