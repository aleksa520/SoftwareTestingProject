using System;
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
            //Connection = new SqlConnection(
        }

        public static DatabaseBroker Session()
        {
            if (Broker == null) Broker = new DatabaseBroker();
            return Broker;
        }

    }
}