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
                cmd.Parameters.Add("@contactFirstName", SqlDbType.NVarChar, 50).Value = contactInfo.ContactFirstName;
                cmd.Parameters.Add("@contactLastName", SqlDbType.NVarChar, 50).Value = contactInfo.ContactLastName;
                cmd.Parameters.Add("@contactPhoneNumber", SqlDbType.VarChar, 8).Value = contactInfo.ContactPhoneNumber ?? (object)DBNull.Value;
                cmd.Parameters.Add("@contactEmail", SqlDbType.NVarChar, 100).Value = contactInfo.ContactEmail ?? (object)DBNull.Value;
                cmd.Parameters.Add("@memberID", SqlDbType.Int).Value = contactInfo.MemberID;
                cmd.ExecuteNonQuery();
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
                int affected = cmd.ExecuteNonQuery();
                if (affected == 0)
                    throw new InvalidOperationException("Updating contact did not affect any rows.");
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
