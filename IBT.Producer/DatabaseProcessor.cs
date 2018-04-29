using System.Data.SqlClient;
using Dapper.Contrib.Extensions;
using IBT.Messaging;

namespace IBT.Router
{
    public class DatabaseProcessor : IDatabaseProcessor
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(DatabaseProcessor));
        public void PersistToDatabase(DbMessage message)
        {
            _log.Info("Persist message to database");

            // Uncomment this to save to a table that has an Identity Id column and the two extra columns from db message

            //using (var sqlConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString))
            //{
            //    sqlConnection.Insert(message);
            //}
        }
    }
}