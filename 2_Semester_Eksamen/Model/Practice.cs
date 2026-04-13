using System;
using System.Collections.Generic;
using System.Text;

namespace _2_Semester_Eksamen.Model
{
    public class Practice
    {
        public string Name { get; set; }

        public DateOnly Date { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public Practice(string name, DateOnly date, TimeOnly startTime, TimeOnly endTime)
        {
            Name = name;
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
