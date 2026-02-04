using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using School.Application.Common.Email;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _config;

    public SmtpEmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendAsync(string to, string subject, string htmlBody)
    {
        var smtp = new SmtpClient
        {
            Host = _config["Email:Smtp:Host"],
            Port = int.Parse(_config["Email:Smtp:Port"]!),
            EnableSsl = true,
            Credentials = new NetworkCredential(
                _config["Email:Smtp:Username"],
                _config["Email:Smtp:Password"]
            )
        };

        var message = new MailMessage
        {
            From = new MailAddress(_config["Email:From"]!),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };

        message.To.Add(to);

        await smtp.SendMailAsync(message);
    }
}
