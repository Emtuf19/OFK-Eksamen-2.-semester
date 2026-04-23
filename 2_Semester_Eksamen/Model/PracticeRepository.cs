using _2_Semester_Eksamen.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq.Expressions;
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
                        //if (practice == null) break;

                        var member = new Member
                        {
                            MemberID = reader["MemberID"] is DBNull ? 0 : Convert.ToInt32(reader["MemberID"]),
                            MemberFirstName = reader["MemberFirstName"] is DBNull ? string.Empty : (string)reader["MemberFirstName"],
                            MemberLastName = reader["MemberLastName"] is DBNull ? string.Empty : (string)reader["MemberLastName"]
                        };

                        practice.Members.Add(member);
                    }
                }

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        //if (practice == null) break;

                        var trainer = new Trainer
                        {
                            TrainerID = reader["TrainerID"] is DBNull ? 0 : Convert.ToInt32(reader["TrainerID"]),
                            TrainerFirstName = reader["TrainerFirstName"] is DBNull ? string.Empty : (string)reader["TrainerFirstName"],
                            TrainerLastName = reader["TrainerLastName"] is DBNull ? string.Empty : (string)reader["TrainerLastName"],
                            TrainerPhoneNumber = reader["TrainerPhoneNumber"] is DBNull ? string.Empty : (string)reader["TrainerPhoneNumber"],
                            TrainerEmail = reader["TrainerEmail"] is DBNull ? string.Empty : (string)reader["TrainerEmail"]
                        };

                        practice.Trainers.Add(trainer);
                    }
                }
                return practice;
            }
        }

        public override List<Practice> GetAll()
        {
            List<Practice> practices = new List<Practice>();

            using (SqlConnection con = CreateConnection())
            {
                con.Open();

                using SqlCommand cmd = new SqlCommand("sp_GetAllPracticesWithMembersAndTrainers", con);

                cmd.CommandType = CommandType.StoredProcedure;

                using SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var practice = new Practice
                    {
                        PracticeID = Convert.ToInt32(reader["PracticeID"]),
                        PracticeName = reader["PracticeName"] is DBNull ? string.Empty : (string)reader["PracticeName"],
                        StartTime = Convert.ToDateTime(reader["StartTime"]),
                        EndTime = Convert.ToDateTime(reader["EndTime"]),
                        Members = new List<Member>(),
                        Trainers = new List<Trainer>()
                    };
                    practices.Add(practice);
                }

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        var PracticeIDObj = reader["PracticeID"];
                        if (PracticeIDObj == DBNull.Value) continue;
                        int practiceID = Convert.ToInt32(PracticeIDObj);

                        var practice = practices.Find(p => p.PracticeID == practiceID);
                        if (practice == null) continue;

                        var MemberIDObject = reader["MemberID"];
                        if (MemberIDObject != DBNull.Value)
                        {
                            var member = new Member
                            {
                                MemberID = Convert.ToInt32(MemberIDObject),
                                MemberFirstName = reader["MemberFirstName"] is DBNull ? string.Empty : (string)reader["MemberFirstName"],
                                MemberLastName = reader["MemberLastName"] is DBNull ? string.Empty : (string)reader["MemberLastName"],
                            };
                            practice.Members.Add(member);
                        }
                    }
                }

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        var PracticeIDObj = reader["PracticeID"];
                        if (PracticeIDObj == DBNull.Value) continue;
                        int practiceID = Convert.ToInt32(PracticeIDObj);

                        var practice = practices.Find(p => p.PracticeID == practiceID);
                        if (practice == null) continue;

                        var trainerIDObject = reader["TrainerID"];
                        if (trainerIDObject != DBNull.Value)
                        {
                            var trainer = new Trainer
                            {
                                TrainerID = Convert.ToInt32(trainerIDObject),
                                TrainerFirstName = reader["TrainerFirstName"] is DBNull ? string.Empty : (string)reader["TrainerFirstName"],
                                TrainerLastName = reader["TrainerLastName"] is DBNull ? string.Empty : (string)reader["TrainerLastName"],
                                TrainerPhoneNumber = reader["TrainerPhoneNumber"] is DBNull ? string.Empty : (string)reader["TrainerPhoneNumber"],
                                TrainerEmail = reader["TrainerEmail"] is DBNull ? string.Empty : (string)reader["TrainerEmail"]
                            };
                            practice.Trainers.Add(trainer);
                        }
                    }
                }
            }
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

                cmd.ExecuteNonQuery();
            }
        }

        public override void Update(Practice practice)
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();

                using SqlCommand cmd = new SqlCommand("dbo.sp_UpdatePractice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PracticeID", SqlDbType.Int).Value = practice.PracticeID;
                cmd.Parameters.Add("@PracticeName", SqlDbType.NVarChar, 50).Value = practice.PracticeName;
                cmd.Parameters.Add("@StartTime", SqlDbType.DateTime2).Value = practice.StartTime;
                cmd.Parameters.Add("@EndTime", SqlDbType.DateTime2).Value = practice.EndTime;
                cmd.ExecuteNonQuery();
            }
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
