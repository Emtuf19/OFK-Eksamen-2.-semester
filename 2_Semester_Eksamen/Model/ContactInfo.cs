using System;
using System.Collections.Generic;
using System.Text;

namespace _2_Semester_Eksamen.Model
{
    public class ContactInfo
    {
        public int ContactPersonID { get; set; }
        public int MemberID { get; set; }
        public string ContactFirstName { get; set; } = string.Empty;
        public string ContactLastName { get; set; } = string.Empty;
        public string ContactPhoneNumber { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
    }
}
