using System.Net;
using System.Net.Mail;
using Covoiturage.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace Covoiturage.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendVerificationCodeAsync(string toEmail, string userName, string code)
    {
        var smtpServer = _config["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
        var smtpPort = int.Parse(_config["EmailSettings:SmtpPort"] ?? "587");
        var senderName = _config["EmailSettings:SenderName"] ?? "CovoitExpress";
        var senderEmail = _config["EmailSettings:SenderEmail"]
            ?? throw new InvalidOperationException("SenderEmail non configuré.");
        var appPassword = _config["EmailSettings:AppPassword"]
            ?? throw new InvalidOperationException("AppPassword non configuré.");

        using var client = new SmtpClient(smtpServer, smtpPort)
        {
            Credentials = new NetworkCredential(senderEmail, appPassword),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(senderEmail, senderName),
            Subject = "Votre code de vérification CovoitExpress",
            IsBodyHtml = true,
            Body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 500px; margin: auto; padding: 20px; border: 1px solid #e2e8f0; border-radius: 10px;'>
                    <h2 style='color: #2563eb;'>Bienvenue sur CovoitExpress, {userName} !</h2>
                    <p style='color: #334155;'>Voici votre code de vérification à 6 chiffres pour activer votre compte :</p>
                    <div style='background-color: #f1f5f9; padding: 15px; text-align: center; font-size: 28px; font-weight: bold; letter-spacing: 6px; color: #0f172a; border-radius: 8px; margin: 20px 0;'>
                        {code}
                    </div>
                    <p style='color: #64748b; font-size: 12px;'>Ce code est valable pendant 15 minutes.</p>
                </div>"
        };

        mailMessage.To.Add(toEmail);

        await client.SendMailAsync(mailMessage);
    }
}