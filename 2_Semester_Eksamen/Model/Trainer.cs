using System;
using System.Collections.Generic;
using System.Text;

namespace _2_Semester_Eksamen.Model
{
    public class Trainer
    {
        public int TrainerID { get; set; }
        public string TrainerFirstName { get; set; }
        public string TrainerLastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public List<Practice> Practices { get; set; } = new List<Practice>();
    }
}
