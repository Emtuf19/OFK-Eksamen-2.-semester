using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace _2_Semester_Eksamen.Model
{
    public class MemberRepository : BaseRepo<Member>
    {
        private List<Member> members = new List<Member>();

        public override Member? GetById(int ID)
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();

                using SqlCommand cmd = new SqlCommand("sp_GetMemberByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@MemberID", SqlDbType.Int).Value = ID;

                using SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                    return null;

                Member? member = null;

                while (reader.Read())
                {
                    if (member == null)
                    {
                        member = new Member
                        {
                            MemberID = Convert.ToInt32(reader["MemberID"]),
                            FirstName = reader["MemberFirstName"] is DBNull ? string.Empty : (string)reader["MemberFirstName"],
                            LastName = reader["MemberLastName"]  is DBNull ? string.Empty : (string)reader["MemberLastName"]
                        };
                    }

                    var contactIDObject = reader["ContactPersonID"];
                    if (contactIDObject != DBNull.Value)
                    {
                        var contact = new ContactInfo
                        {
                            ContactPersonID = Convert.ToInt32(contactIDObject),
                            FirstName = reader["ContactFirstName"] is DBNull ? string.Empty : (string)reader["ContactFirstName"],
                            LastName = reader["ContactLastName"]  is DBNull ? string.Empty : (string)reader["ContactLastName"],
                            ContactPhoneNumber = reader["ContactPhoneNumber"] is DBNull ? string.Empty : (string)reader["ContactPhoneNumber"],
                            ContactEmail = reader["ContactEmail"] is DBNull ? string.Empty : (string)reader["ContactEmail"]
                        };

                        member.ContactPersons.Add(contact);
                    }
                }

                return member;
            }
        }

        public override List<Member> GetAll()
        {
            return members;
        }

        public override void Add(Member member)
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();

                using SqlCommand cmd = new SqlCommand("dbo.sp_InsertIntoTrainer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TrainerFirstName", SqlDbType.NVarChar, 50).Value = member.FirstName;
                cmd.Parameters.Add("@TrainerLastName", SqlDbType.NVarChar, 50).Value = member.LastName;
            }
        }

        public override void Update(Member member)
        {

        }

        public override void Delete(int ID)
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("DELETE FROM Member WHERE MemberID = @ID", con))
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}