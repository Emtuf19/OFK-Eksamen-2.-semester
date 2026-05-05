using System;
using System.Collections.Generic;
using System.Text;

namespace _2_Semester_Eksamen.Model
{
    public class Member
    {
        public int MemberID { get; set; }
        public string MemberFirstName { get; set; } = string.Empty;
        public string MemberLastName { get; set; } = string.Empty;

        public string FullName => $"{MemberFirstName} {MemberLastName}".Trim();

        public List<ContactInfo> ContactPersons { get; set; } = new List<ContactInfo>();
    }
}
