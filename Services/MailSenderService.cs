using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using CI_mainservice.Settings;
using CI_mainservice.Models;
namespace CI_mainservice.Services
{
    public class MailSenderService
    {
        private readonly HostMail _settings;

        public MailSenderService(IOptions<HostMail> options)
        {
            _settings = options.Value;
        }
        public async Task SendEmailAsync(ClientsToDB client_info, Category_problems problem, string subject = "Обращение в тех.поддержку")
        {
            var emailMessage = new MimeMessage();
            string htmlTemplate = File.ReadAllText("Services/template.html");
            string htmlText = htmlTemplate.Replace("{{MESSAGE}}", problem.response);

            emailMessage.From.Add(new MailboxAddress(_settings.hostname, _settings.mailname));
            emailMessage.To.Add(new MailboxAddress(client_info.first_name + " " + client_info.last_name + " " + client_info.father_name, client_info.email));
            emailMessage.Subject = subject;
            emailMessage.Body = new MultipartAlternative {
                new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlText }
            };


            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com",587, MailKit.Security.SecureSocketOptions.StartTls);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_settings.mailname, _settings.password_app);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }

}
