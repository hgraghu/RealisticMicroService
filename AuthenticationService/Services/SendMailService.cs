using AuthenticationService.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AuthenticationService.Services
{
    public class SendMailService : IMailService
    {
        private readonly IConfiguration configuration;

        public SendMailService(IConfiguration Configuration)
        {
            configuration = Configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var apiKey = configuration["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("hgraghu@gmail.com", "Welcome to BlackFowl!...");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
        }
    }
}
