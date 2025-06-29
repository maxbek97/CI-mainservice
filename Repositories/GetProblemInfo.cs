using CI_mainservice.Settings;
using CI_mainservice.Models;
using Dapper;
using MySql.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Options;


namespace CI_mainservice.Repositories
{
    public class GetProblemInfo
    {
        private readonly ConnectionStrings _settings;
        public DapperMethods query_carry;
        public GetProblemInfo(ConnectionStrings _options)
        {
            query_carry = new DapperMethods(_options);
        }
        public int category_id { get; set; }
        public Category_problems GetCategory(string name_of_problem)
        {
            Console.WriteLine(name_of_problem);
            var sql = "SELECT id_response, problem_name, response FROM Category_problems WHERE problem_name = @problema";
            var ans = query_carry.Query<Category_problems>(sql, new { problema = name_of_problem }).ToList();
            return ans[0];
        }
    }
}
