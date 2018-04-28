using System.Data.SqlClient;
using Dapper.Contrib.Extensions;
using IBT.Messaging;

namespace IBT.Processor
{
    public class DatabaseProcessor : IDatabaseProcessor
    {
        public void PersistToDatabase(DbMessage message)
        {
            using (var sqlConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString))
            {
                sqlConnection.Insert(message);
            }
        }
    }

    public interface IDatabaseProcessor
    {
        void PersistToDatabase(DbMessage message);
    }
}