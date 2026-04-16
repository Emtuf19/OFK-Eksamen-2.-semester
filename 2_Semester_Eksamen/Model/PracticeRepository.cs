using _2_Semester_Eksamen.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;

namespace _2_Semester_Eksamen.Model
{
    public class PracticeRepository : BaseRepo<Practice>
    {
        private List<Practice> practices = new List<Practice>();

        public override Practice? GetById(int ID)
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();

                Practice practice = new Practice();

                using SqlCommand cmd = new SqlCommand("dbo.sp_GetPracticeByIDWithMembersAndTrainers", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;

                using SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                    return null;

                if (reader.Read())
                {
                    practice = new Practice
                    {
                        PracticeID = Convert.ToInt32(reader["PracticeID"]),
                        PracticeName = reader["PracticeName"] is DBNull ? string.Empty : (string)reader["PracticeName"],
                        StartTime = reader["StartTime"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["StartTime"]),
                        EndTime = reader["EndTime"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["EndTime"])
                    };
                }

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        if (practice == null) break;

                        var member = new Member
                        {
                            MemberID = reader["MemberID"] is DBNull ? 0 : Convert.ToInt32(reader["MemberID"]),
                            FirstName = reader["MemberFirstName"] is DBNull ? string.Empty : (string)reader["MemberFirstName"],
                            LastName = reader["MemberLastName"] is DBNull ? string.Empty : (string)reader["MemberLastName"]
                        };

                        practice.Members.Add(member);
                    }
                }

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        if (practice == null) break;

                        var trainer = new Trainer
                        {
                            TrainerID = reader["TrainerID"] is DBNull ? 0 : Convert.ToInt32(reader["TrainerID"]),
                            FirstName = reader["TrainerFirstName"] is DBNull ? string.Empty : (string)reader["TrainerFirstName"],
                            LastName = reader["TrainerLastName"] is DBNull ? string.Empty : (string)reader["TrainerLastName"],
                            Email = reader["TrainerEmail"] is DBNull ? string.Empty : (string)reader["TrainerEmail"]
                        };

                        practice.Trainers.Add(trainer);
                    }
                }


                return practice;
            }
        }

        public override List<Practice> GetAll()
        {
            return practices;
        }

        public override void Add(Practice practice)
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();

                using SqlCommand cmd = new SqlCommand("dbo.sp_InsertIntoPractice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PracticeName", SqlDbType.NVarChar, 50).Value = practice.PracticeName;
                cmd.Parameters.Add("@StartTime", SqlDbType.DateTime2).Value = practice.StartTime;
                cmd.Parameters.Add("@EndTime", SqlDbType.DateTime2).Value = practice.EndTime;
            }
        }

        public override void Update(Practice practice)
        {

        }

        public override void Delete(int ID)
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("DELETE FROM Practice WHERE PracticeID = @ID", con))
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }

}
