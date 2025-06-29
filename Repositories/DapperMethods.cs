using MySql.Data.MySqlClient;
using Dapper;
using Microsoft.Extensions.Options;
using CI_mainservice.Settings;

namespace CI_mainservice.Repositories
{

    public class DapperMethods
    {
        private readonly string _connectionString;

        public DapperMethods(ConnectionStrings options)
        {
            _connectionString = options.Databases.MainDB;
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public void ExecuteQuery(string query)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                connection.Execute(query);
            }
        }
        public void ExecuteQuery(string query, object parameters)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    connection.Execute(query, parameters);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public T QuerySingle<T>(string query)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                return connection.QuerySingle<T>(query);
            }
        }

        public IEnumerable<T> Query<T>(string query, object parameters = null)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                return connection.Query<T>(query, parameters);
            }
        }
    }
}
