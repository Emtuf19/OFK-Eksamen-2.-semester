using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Text;

namespace _2_Semester_Eksamen.Model
{
    public class ContactInfoRepository : BaseRepo<ContactInfo>
    {
        private List<ContactInfo> contactInfos = new List<ContactInfo>();

        public override ContactInfo? GetById(int ID)
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();

                ContactInfo contactInfo = new ContactInfo();

                using SqlCommand cmd = new SqlCommand("dbo.GetByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;

                return contactInfo;
            }
        }

        public override List<ContactInfo> GetAll()
        {
            return contactInfos;
        }

        public override void Add(ContactInfo contactInfo)
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();

                using SqlCommand cmd = new SqlCommand("dbo.sp_InsertIntoContactInfo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TrainerFirstName", SqlDbType.NVarChar, 50).Value = contactInfo.FirstName;
                cmd.Parameters.Add("@TrainerLastName", SqlDbType.NVarChar, 50).Value = contactInfo.LastName;
                cmd.Parameters.Add("@PhoneNumber", SqlDbType.Int).Value = contactInfo.ContactPhoneNumber;
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = contactInfo.ContactEmail;
            }
        }

        public override void Update(ContactInfo contactInfo)
        {

        }

        public override void Delete(int ID)
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("DELETE FROM ContactInfo WHERE ContactInfoID = @ID", con))
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                    cmd.ExecuteNonQuery();
                }
            }


        }
    }
}
