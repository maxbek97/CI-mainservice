using System.Text.RegularExpressions;
using CI_mainservice.Models;
using CI_mainservice.Settings;
using Microsoft.Extensions.Options;
using CI_mainservice.Repositories;
namespace CI_mainservice.Services
{
    public class PredictResponse
    {
        public string category { get; set; }
    }
    public class FIO
    {
        public string first_name { get; private set; }
        public string last_name { get; private set; }
        public string father_name { get; private set; }
        public FIO(string unparsedstr)
        {
            get_names(unparsedstr);
        }
        private void get_names(string unparsedstr)
        {
            List<String> names = new List<String>();
            unparsedstr = Regex.Replace(unparsedstr, "[^а-яА-ЯёЁ ]", "");
            names = unparsedstr.Split().ToList();
            first_name = names[0];
            last_name = names[1];
            father_name = names[2];
        }
    }

    public class MainService
    {
        private readonly ConnectionStrings _settings;
        private readonly IOptions<HostMail> _settings_mail;

        public ClientRequest clientData { private get; init; }
        public MainService(ConnectionStrings options, IOptions<HostMail> maildata, ClientRequest data_from_controller)
        {
            _settings_mail = maildata;
            _settings = options;
            clientData = data_from_controller;
        }

        public async Task<string> GetCategoryName()
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsJsonAsync(_settings.ExtServices.AI_service, new { message = clientData.Message });
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<PredictResponse>();
                //Console.WriteLine($"📍 Категория: {result.category}");
                return result.category;
            }
            else
            {
                //Console.WriteLine($"❌ Ошибка: {response.StatusCode}");
                return response.StatusCode.ToString();
            }
        }

        public async void main_process()
        {
            Category_problems problem_info = new GetProblemInfo(_settings).GetCategory(await GetCategoryName());
            
            //Вставка в бд
            var client_data = await prepare_client_info_to_DB(problem_info);
            //Обратиться к методу из репозитория, который отправит в БД данные
            InsertClientData insertion = new InsertClientData(_settings);
            insertion.Insert(client_data);

            //Почта
            try
            {
                var mailsender = new MailSenderService(_settings_mail);
                await mailsender.SendEmailAsync(client_data, problem_info);
            }
            catch (Exception e)
            {
                Console.WriteLine("Опять гугл выэтосамывается " + e.Message);
            }
        }

        private async Task<ClientsToDB> prepare_client_info_to_DB(Category_problems problem)
        {
            ClientsToDB client_info = new ClientsToDB();
            FIO client_namse = new FIO(clientData.Fio);
            client_info.first_name = client_namse.first_name;
            client_info.last_name = client_namse.last_name;
            client_info.father_name = client_namse.father_name;
            client_info.email = clientData.Email;
            client_info.telephone = clientData.Phone;
            Category_problems category_problem = new GetProblemInfo(_settings).GetCategory(await GetCategoryName());
            client_info.category_id = category_problem.id_response;
            return client_info;
        }
    }

}
