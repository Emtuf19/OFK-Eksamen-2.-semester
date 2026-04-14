using _2_Semester_Eksamen.Model;
using Microsoft.Data.SqlClient;
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

                using SqlCommand cmd = new SqlCommand("dbo.GetByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;

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
