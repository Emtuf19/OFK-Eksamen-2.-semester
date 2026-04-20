using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace _2_Semester_Eksamen.Model
{
    public class TrainerRepository : BaseRepo<Trainer>
    {
        private List<Trainer> trainers = new List<Trainer>();

        public override Trainer? GetById(int ID)
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();

                Trainer? trainer = null;

                using SqlCommand cmd = new SqlCommand("dbo.GetByTrainerID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;

                using SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    trainer = new Trainer
                    {
                        TrainerID = Convert.ToInt32(reader["TrainerID"]),
                        TrainerFirstName = reader["TrainerFirstName"] is DBNull ? string.Empty : (string)reader["TrainerFirstName"],
                        TrainerLastName = reader["TrainerLastName"] is DBNull ? string.Empty : (string)reader["TrainerLastName"],
                        TrainerPhoneNumber = reader["TrainerPhoneNumber"] is DBNull ? string.Empty : (string)reader["TrainerPhoneNumber"],
                        TrainerEmail = reader["TrainerEmail"] is DBNull ? string.Empty : (string)reader["TrainerEmail"]
                    };
                }
                return trainer;
            }
        }

        public override List<Trainer> GetAll()
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();
                trainers = new List<Trainer>();

                using SqlCommand cmd = new SqlCommand("sp_GetAllTrainers", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var trainer = new Trainer
                    {
                        TrainerID = Convert.ToInt32(reader["TrainerID"]),
                        TrainerFirstName = reader["TrainerFirstName"] is DBNull ? string.Empty : (string)reader["TrainerFirstName"],
                        TrainerLastName = reader["TrainerLastName"] is DBNull ? string.Empty : (string)reader["TrainerLastName"],
                        TrainerPhoneNumber = reader["TrainerPhoneNumber"] is DBNull ? string.Empty : (string)reader["TrainerPhoneNumber"],
                        TrainerEmail = reader["TrainerEmail"] is DBNull ? string.Empty : (string)reader["TrainerEmail"]
                    };
                    trainers.Add(trainer);
                }
                return trainers;
            }
        }

        public override void Add(Trainer trainer)
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();

                using SqlCommand cmd = new SqlCommand("dbo.sp_InsertIntoTrainer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TrainerFirstName", SqlDbType.NVarChar, 50).Value = trainer.TrainerFirstName;
                cmd.Parameters.Add("@TrainerLastName", SqlDbType.NVarChar, 50).Value = trainer.TrainerLastName;
                cmd.Parameters.Add("@TrainerPhoneNumber", SqlDbType.Int).Value = trainer.TrainerPhoneNumber;
                cmd.Parameters.Add("@TrainerEmail", SqlDbType.NVarChar, 50).Value = trainer.TrainerEmail;
            }
        }

        public override void Update(Trainer trainer)
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();

                using SqlCommand cmd = new SqlCommand("dbo.sp_UpdateTrainer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TrainerID", SqlDbType.Int).Value = trainer.TrainerID;
                cmd.Parameters.Add("@TrainerFirstName", SqlDbType.NVarChar, 50).Value = trainer.TrainerFirstName;
                cmd.Parameters.Add("@TrainerLastName", SqlDbType.NVarChar, 50).Value = trainer.TrainerLastName;
                cmd.Parameters.Add("@TrainerPhoneNumber", SqlDbType.Int).Value = trainer.TrainerPhoneNumber;
                cmd.Parameters.Add("@TrainerEmail", SqlDbType.NVarChar, 50).Value = trainer.TrainerEmail;
                cmd.ExecuteNonQuery();
            }
        }

        public override void Delete(int ID)
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("DELETE FROM Trainer WHERE TrainerID = @ID", con))
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
