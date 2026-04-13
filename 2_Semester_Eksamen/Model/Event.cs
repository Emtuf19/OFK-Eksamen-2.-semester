using System;
using System.Collections.Generic;
using System.Text;

namespace _2_Semester_Eksamen.Model
{
    class Event
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public string AgeGroup { get; set; }

        public DateTime Time { get; set; }

        public Event(string name, string description, double price, string ageGroup, DateTime time)
        {
            Name = name;
            Description = description;
            Price = price;
            AgeGroup = ageGroup;
            Time = time;
        }
    }
}
