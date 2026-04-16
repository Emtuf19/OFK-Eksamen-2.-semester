using System;
using System.Collections.Generic;
using System.Text;

namespace _2_Semester_Eksamen.Model
{
    public class Event
    {
        public int EventID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string AgeGroup { get; set; }
        public DateTime Time { get; set; }

        public List<Member> Members { get; set; } = new List<Member>();
        public List<Trainer> Trainers { get; set; } = new List<Trainer>();
    }
}
