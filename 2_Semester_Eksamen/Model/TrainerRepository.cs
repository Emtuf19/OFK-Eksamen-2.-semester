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

                Trainer trainer = new Trainer();

                using SqlCommand cmd = new SqlCommand("dbo.GetByID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;

                return trainer;
            }
        }

        public override List<Trainer> GetAll()
        {
            return trainers;
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
                cmd.Parameters.Add("@PhoneNumber", SqlDbType.Int).Value = trainer.PhoneNumber;
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = trainer.Email;
            }
        }

        public override void Update(Trainer trainer)
        {

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
