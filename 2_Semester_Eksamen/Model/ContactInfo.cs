using System;
using System.Collections.Generic;
using System.Text;

namespace _2_Semester_Eksamen.Model
{
    public class ContactInfo
    {
        public string ContactName { get; set; }

        public string ContactPhoneNumber { get; set; }

        public string ContactEmail { get; set; }

        public ContactInfo(string contactName, string contactPhoneNumber, string contactEmail)
        {
            ContactName = contactName;
            ContactPhoneNumber = contactPhoneNumber;
            ContactEmail = contactEmail;
        }
    }
}
