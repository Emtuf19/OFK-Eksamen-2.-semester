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

        public override List<ContactInfo> GetAll() => throw new NotImplementedException();

        public override void Add(ContactInfo contactInfo)
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();

                using SqlCommand cmd = new SqlCommand("dbo.sp_InsertIntoContactInfo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@MemberFirstName", SqlDbType.NVarChar, 50).Value = contactInfo.ContactFirstName;
                cmd.Parameters.Add("@MemberLastName", SqlDbType.NVarChar, 50).Value = contactInfo.ContactLastName;
                cmd.Parameters.Add("@ContactPhoneNumber", SqlDbType.Int).Value = contactInfo.ContactPhoneNumber;
                cmd.Parameters.Add("@ContactEmail", SqlDbType.NVarChar, 50).Value = contactInfo.ContactEmail;
                cmd.Parameters.Add("@MemberID", SqlDbType.Int).Value = contactInfo.MemberID;
            }
        }

        public override void Update(ContactInfo contactInfo)
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();

                using SqlCommand cmd = new SqlCommand("dbo.sp_UpdateContactInfo", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ContactPersonID", SqlDbType.Int).Value = contactInfo.ContactPersonID;
                cmd.Parameters.Add("@ContactFirstName", SqlDbType.NVarChar, 50).Value = contactInfo.ContactFirstName;
                cmd.Parameters.Add("@ContactLastName", SqlDbType.NVarChar, 50).Value = contactInfo.ContactLastName;
                cmd.Parameters.Add("@ContactPhoneNumber", SqlDbType.Int).Value = contactInfo.ContactPhoneNumber;
                cmd.Parameters.Add("@ContactEmail", SqlDbType.NVarChar, 50).Value = contactInfo.ContactEmail;
                cmd.ExecuteNonQuery();
            }
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
