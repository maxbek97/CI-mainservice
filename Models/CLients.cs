using System.ComponentModel.DataAnnotations;

namespace CI_mainservice.Models
{
    public class ClientRequest
    {
        public string Fio { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
    public class ClientsToDB
    {
        public int id_client { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string father_name { get; set; }
        public string telephone { get; set; }
        public string email { get; set; }
        public ushort category_id { get; set; }
    }
    public class Category_problems
    {
        public UInt16 id_response { get; set; }
        public string problem_name { get; set; }
        public string response { get; set; }
    }
}
