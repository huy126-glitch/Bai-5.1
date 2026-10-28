using Microsoft.Data.SqlClient;

namespace Bai41Login
{
    public static class Database
    {
        private static readonly string connectionString =
            @"Server=DESKTOP-TSRKH98\SQLEXPRESS;Database=Bai41LoginDB;Integrated Security=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}