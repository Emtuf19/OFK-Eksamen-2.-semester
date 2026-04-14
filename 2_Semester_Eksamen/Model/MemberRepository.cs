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

                Member member = new Member();

                using SqlCommand cmd = new SqlCommand("dbo.GetByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;

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