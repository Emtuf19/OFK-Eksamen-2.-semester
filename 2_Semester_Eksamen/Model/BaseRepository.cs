using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Internal;
using System.Dynamic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace _2_Semester_Eksamen.Model
{
    public abstract class BaseRepository<TEntity>
        where TEntity : class, new()
    {
        protected List<TEntity> entities;


        protected readonly string ConnectionString;

        protected BaseRepository()
        {
            IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsetting.json").Build();

            entities = new List<TEntity>();
            ConnectionString = config.GetConnectionString("MyDBConnection");
        }

        protected SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }


        public abstract int GetById(int entity);

        public abstract List<TEntity> GetAll();

        public abstract void Add(TEntity entity);

        public abstract void Update(TEntity entity);

        public abstract void Delete(int entity);
    }
}
