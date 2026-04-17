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

                Event? event1 = null;

                using SqlCommand cmd = new SqlCommand("sp_GetEventByIDWithMembersAndTrainers", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EventID", SqlDbType.Int).Value = ID;

                using SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                    return null;

                if (reader.Read())
                {
                    event1 = new Event
                    {
                        EventID = Convert.ToInt32(reader["EventID"]),
                        Name = reader["EventName"] is DBNull ? string.Empty : (string)reader["EventName"],
                        Description = reader["Description"] is DBNull ? string.Empty : (string)reader["Description"],
                        Price = reader["Price"] is DBNull ? 0.0 : Convert.ToDouble(reader["Price"]),
                        AgeGroup = reader["AgeGroup"] is DBNull ? string.Empty : (string)reader["AgeGroup"],
                        Time = reader["Time"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["Time"])
                    };
                }

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        if (event1 == null) break;

                        var member = new Member
                        {
                            MemberID = reader["MemberID"] is DBNull ? 0 : Convert.ToInt32(reader["MemberID"]),
                            FirstName = reader["MemberFirstName"] is DBNull ? string.Empty : (string)reader["MemberFirstName"],
                            LastName = reader["MemberLastName"] is DBNull ? string.Empty : (string)reader["MemberLastName"]
                        };

                        event1.Members.Add(member);
                    }
                }

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        if (event1 == null) break;

                        var trainer = new Trainer
                        {
                            TrainerID = reader["TrainerID"] is DBNull ? 0 : Convert.ToInt32(reader["TrainerID"]),
                            FirstName = reader["TrainerFirstName"] is DBNull ? string.Empty : (string)reader["TrainerFirstName"],
                            LastName = reader["TrainerLastName"] is DBNull ? string.Empty : (string)reader["TrainerLastName"],
                            Email = reader["TrainerEmail"] is DBNull ? string.Empty : (string)reader["TrainerEmail"]
                        };

                        event1.Trainers.Add(trainer);
                    }
                }
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
