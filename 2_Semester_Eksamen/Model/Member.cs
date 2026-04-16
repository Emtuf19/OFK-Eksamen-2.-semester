using System;
using System.Collections.Generic;
using System.Text;

namespace _2_Semester_Eksamen.Model
{
    public class Member
    {
        public int MemberID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<ContactInfo> ContactPersons { get; set; } = new List<ContactInfo>();
    }
}
