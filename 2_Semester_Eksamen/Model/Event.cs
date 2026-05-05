using System;
using System.Collections.Generic;
using System.Text;

namespace _2_Semester_Eksamen.Model
{
    public class Event
    {
        public int EventID { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AgeGroup { get; set; } = string.Empty;
        public double Price { get; set; }      
        public DateTime Time { get; set; }

        public List<Member> Members { get; set; } = new();
        public List<Trainer> Trainers { get; set; } = new();

    }
}
