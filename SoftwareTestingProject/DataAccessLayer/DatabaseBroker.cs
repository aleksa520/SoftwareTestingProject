﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareTestingProject.DataAccessLayer
{
    public class DatabaseBroker
    {
        public SqlCommand Command { get; set; }
        public SqlConnection Connection { get; set; }
        public SqlTransaction Transaction { get; set; }
        public static DatabaseBroker Broker { get; set; }

        DatabaseBroker()
        {
            Connect();
        }

        void Connect()
        {
            Connection = new SqlConnection(@"Data Source=DESKTOP-REGCU4G\SQLEXPRESS;Initial Catalog=SoftwareTestingDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        public static DatabaseBroker Session()
        {
            if (Broker == null) Broker = new DatabaseBroker();
            return Broker;
        }

        public int GetId(string column, string table)
        {
            try
            {
                Connection.Open();
                Command = new SqlCommand("", Connection, Transaction);
                Command.CommandText = $"Select max({column}) from {table}";
                int id = Convert.ToInt32(Command.ExecuteScalar());
                return id + 1;
            }
            catch (Exception)
            {
                return 1;
            }
            finally { if (Connection != null) Connection.Close(); }
        }
    }
}