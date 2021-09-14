using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ExternalAuthentication.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string fromEmail = _configuration["EmailService:Gmail:Email"];
            string fromPassword = _configuration["EmailService:Gmail:Password"];

            MailMessage message = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = "<html><body> " + htmlMessage + " </body></html>",
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(email));

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true,
            };

            smtpClient.Send(message);

            return Task.CompletedTask;
        }
    }
}
