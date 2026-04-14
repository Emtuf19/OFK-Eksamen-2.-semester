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
    public abstract class BaseRepo<TEntity>
        where TEntity : class, new()
    {
        protected List<TEntity> entities;


        protected readonly string ConnectionString;

        protected BaseRepo()
        {
            IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            entities = new List<TEntity>();
            ConnectionString = config.GetConnectionString("MyDBConnection");
        }

        protected SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public abstract TEntity GetById(int entity);

        public abstract List<TEntity> GetAll();

        public abstract void Add(TEntity entity);

        public abstract void Update(TEntity entity);

        public abstract void Delete(int entity);
    }
}
