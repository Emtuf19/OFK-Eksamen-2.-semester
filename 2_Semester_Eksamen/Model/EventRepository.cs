using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace _2_Semester_Eksamen.Model
{
    public class EventRepository : BaseRepo<Event>
    {
        private List<Event> events = new List<Event>();

        public override Event? GetById(int ID)
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();

                Event event1 = new Event();

                using SqlCommand cmd = new SqlCommand("dbo.GetByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;

                return event1;
            }
        }

        public override List<Event> GetAll()
        {
            return events;
        }

        public override void Add(Event event1)
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();

                using SqlCommand cmd = new SqlCommand("dbo.sp_InsertIntoEvent", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EventName", SqlDbType.NVarChar, 50).Value = event1.Name;
                cmd.Parameters.Add("@EventName", SqlDbType.NVarChar, 1000).Value = event1.Description;
                cmd.Parameters.Add("@EventName", SqlDbType.Float).Value = event1.Price;
                cmd.Parameters.Add("@EventName", SqlDbType.NVarChar, 50).Value = event1.AgeGroup;
                cmd.Parameters.Add("@StartTime", SqlDbType.DateTime2).Value = event1.Time;
            }
        }

        public override void Update(Event event1)
        {

        }

        public override void Delete(int ID)
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("DELETE FROM Event WHERE EventID = @ID", con))
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
