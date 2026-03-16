using Microsoft.Data.SqlClient;
// Created using dotnet add package Microsoft.Data.SqlClient
using System.Data;

namespace Core
{
    internal sealed class DbConnection: IDisposable
    {
        private readonly SqlConnection _connection;

        /// <summary>
        /// Constructor for DbConnection.
        /// Builds a connection string using the default values:
        /// localhost\\SQLEXPRESS, ContactsManagerDb, sa, root.
        /// </summary>
        public DbConnection()
        {
            // Build connection string INSIDE constructor
            string server = "localhost\\SQLEXPRESS";
            string database = "ContactsManagerDb";
            string user = "sa";
            string pass = "root";

            string connectionString =
                $"Server={server};" +
                $"Database={database};" +
                $"User Id={user};" +
                $"Password={pass};" +
                $"TrustServerCertificate=True;";

            _connection = new SqlConnection(connectionString);
        }

        internal SqlConnection GetConnection()
        {
            return _connection;
        }

        internal async Task<SqlConnection> OpenAsync()
        {
            if (_connection.State != ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }
            return _connection;
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
        
    }
}
