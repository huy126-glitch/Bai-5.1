
using Microsoft.Data.SqlClient;

namespace Bai42Registration
{
    public static class Database
    {
        private static readonly string connectionString =
            @"Server=DESKTOP-TSRKH98\SQLEXPRESS;Database=Bai42RegistrationDB;Integrated Security=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
