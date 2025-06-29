using CI_mainservice.Models;
using CI_mainservice.Settings;

namespace CI_mainservice.Repositories
{
    public class InsertClientData
    {
        private readonly ConnectionStrings _settings;
        public DapperMethods query_carry;
        public InsertClientData(ConnectionStrings _options)
        {
            query_carry = new DapperMethods(_options);
        }
        public void Insert(ClientsToDB client_info)
        {
            var sql = "INSERT INTO clients (first_name, last_name, father_name, telephone, email, category_id) VALUES (@FirstName, @LastName, @FatherName, @Telephone, @Email, @CategoryID)";
            var parameters = new
            {
                FirstName = client_info.first_name,
                LastName = client_info.last_name,
                FatherName = client_info.father_name,
                Telephone = client_info.telephone,
                Email = client_info.email,
                CategoryID = client_info.category_id
            };
            query_carry.ExecuteQuery(sql, parameters);
        }
    }
}
