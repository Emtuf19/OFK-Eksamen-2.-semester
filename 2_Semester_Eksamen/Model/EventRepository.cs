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

        public override List<Event> GetAll()
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();
                events = new List<Event>();

                using SqlCommand cmd = new SqlCommand("sp_GetAllEventsWithMembersAndTrainers", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var event1 = new Event
                    {
                        EventID = Convert.ToInt32(reader["EventID"]),
                        EventName = reader["EventName"] is DBNull ? string.Empty : (string)reader["EventName"],
                        Description = reader["Description"] is DBNull ? string.Empty : (string)reader["Description"],
                        Price = Convert.ToDouble(reader["Price"]),
                        AgeGroup = reader["AgeGroup"] is DBNull ? string.Empty : (string)reader["AgeGroup"],
                        Time = Convert.ToDateTime(reader["Time"]),
                        Members = new List<Member>(),
                        Trainers = new List<Trainer>()
                    };
                    events.Add(event1);
                }

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        var EventIDObj = reader["EventID"];
                        if (EventIDObj == DBNull.Value) continue;
                        int EventID = Convert.ToInt32(EventIDObj);

                        var event1 = events.Find(e => e.EventID == EventID);
                        if (event1 == null) continue;

                        var MemberIDObject = reader["MemberID"];
                        if (MemberIDObject != DBNull.Value)
                        {
                            var member = new Member
                            {
                                MemberID = Convert.ToInt32(MemberIDObject),
                                MemberFirstName = reader["MemberFirstName"] is DBNull ? string.Empty : (string)reader["MemberFirstName"],
                                MemberLastName = reader["MemberLastName"] is DBNull ? string.Empty : (string)reader["MemberLastName"],
                            };
                            event1.Members.Add(member);
                        }
                    }
                }

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        var EventIDObj = reader["EventID"];
                        if (EventIDObj == DBNull.Value) continue;
                        int EventID = Convert.ToInt32(EventIDObj);

                        var event1 = events.Find(e => e.EventID == EventID);
                        if (event1 == null) continue;

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
                            event1.Trainers.Add(trainer);
                        }
                    }
                }
                return events;
            }
        }

        public override void Update(Event event1)
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();

                using SqlCommand cmd = new SqlCommand("dbo.sp_UpdateEvent", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EventID", SqlDbType.Int).Value = event1.EventID;
                cmd.Parameters.Add("@EventName", SqlDbType.NVarChar, 50).Value = event1.EventName;
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 1000).Value = event1.Description;
                cmd.Parameters.Add("@Price", SqlDbType.Float).Value = event1.Price;
                cmd.Parameters.Add("@AgeGroup", SqlDbType.NVarChar, 50).Value = event1.AgeGroup;
                cmd.Parameters.Add("@Time", SqlDbType.DateTime2).Value = event1.Time;
                cmd.ExecuteNonQuery();
            }
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

        public override void Add(Event ev)
        {
            using (SqlConnection conn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand("sp_InsertIntoEvent", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@eventName", SqlDbType.NVarChar, 100).Value = ev.EventName;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar, 250).Value = ev.Description;
                cmd.Parameters.Add("@price", SqlDbType.Float).Value = ev.Price;
                cmd.Parameters.Add("@ageGroup", SqlDbType.NVarChar, 20).Value = ev.AgeGroup;
                cmd.Parameters.Add("@time", SqlDbType.DateTime2).Value = ev.Time;

                // OUTPUT parameter
                SqlParameter outputId = cmd.Parameters.Add("@newEventID", SqlDbType.Int);
                outputId.Direction = ParameterDirection.Output;

                conn.Open();
                cmd.ExecuteNonQuery();

                // Sæt ID tilbage på objektet
                ev.EventID = (int)outputId.Value;
            }
        }

        //metode til at fjerne medlemmer fra Event
        public void RemoveMemberFromEvent(int eventID, int memberID)
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();
                using SqlCommand cmd = new SqlCommand("sp_CancelSignUpEvent", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EventID", SqlDbType.Int).Value = eventID;
                cmd.Parameters.Add("@MemberID", SqlDbType.Int).Value = memberID;
                cmd.ExecuteNonQuery();
            }
        }

        public void AddMemberToEvent(int eventID, int memberID)
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();
                using SqlCommand cmd = new SqlCommand("sp_SignUpEvent", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@EventID", SqlDbType.Int).Value = eventID;
                cmd.Parameters.Add("@MemberID", SqlDbType.Int).Value = memberID;

                cmd.ExecuteNonQuery();
            }
        }
    }
}
