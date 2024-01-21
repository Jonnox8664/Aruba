// Initialize data for DB connection

using Microsoft.Data.SqlClient;

namespace DBAccess.Mapper
{
    public class BaseMapper
    {
        protected SqlConnection connection;

        public BaseMapper()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            // DB connection data
            builder.DataSource = "192.168.178.104,9433";
            builder.UserID = "sa";
            builder.Password = "P4$sw0rd!|";
            builder.InitialCatalog = "Aruba";
            builder.TrustServerCertificate = true;

            connection = new SqlConnection(builder.ConnectionString);
        }
    }
}