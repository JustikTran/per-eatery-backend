using Application.IService;
using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MimeKit;
using Domain.Entity;

namespace Infrastructure.Service
{
    public class ElasticEmailService : IElasticEmailService
    {
        private readonly IConfiguration _config;

        public ElasticEmailService(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendEmailAsync(VerifyCode verifyCode, string templatePath)
        {
            var htmlBody = await File.ReadAllTextAsync(templatePath);

            //var code = Random.Shared.Next(100000, 999999).ToString();

            htmlBody = htmlBody.Replace("{{Name}}", verifyCode.Name)
                       .Replace("{{Code}}", verifyCode.Code);

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["ElasticEmail:UserName"]));
            email.To.Add(MailboxAddress.Parse(verifyCode.ToMail));
            email.Subject = verifyCode.Subject;

            var builder = new BodyBuilder { HtmlBody = htmlBody };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config["ElasticEmail:Server"], int.Parse(_config["ElasticEmail:Port"] ?? "2525"), MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config["ElasticEmail:UserName"], _config["ElasticEmail:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
