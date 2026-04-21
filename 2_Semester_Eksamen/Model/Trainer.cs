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
        public string TrainerPhoneNumber { get; set; }
        public string TrainerEmail { get; set; }

        public List<Practice> Practices { get; set; } = new List<Practice>();
    }
}
