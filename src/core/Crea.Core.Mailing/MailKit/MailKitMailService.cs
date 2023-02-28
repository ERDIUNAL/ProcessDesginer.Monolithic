﻿using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Crea.Core.Mailing.MailKit;

public class MailKitMailService : IMailService
{
    private MailSettings _mailSettings;

    public MailKitMailService(IConfiguration configuration)
    {
        _mailSettings = configuration.GetRequiredSection("MailSettings").Get<MailSettings>() ?? throw new ArgumentNullException(nameof(MailSettings));
    }

    public async Task SendAsync(Mail mailData)
    {
        MimeMessage mailToSend = new();

        mailToSend.From.Add(new MailboxAddress(_mailSettings.SenderFullName, _mailSettings.SenderEmail));
        mailToSend.To.Add(new MailboxAddress(mailData.ToFullName, mailData.ToEmail));
        mailToSend.Subject = mailData.Subject;

        BodyBuilder bodyBuilder = new()
        {
            TextBody = mailData.TextBody,
            HtmlBody = mailData.HtmlBody
        };

        if (mailData.Attachments is not null)
        {
            foreach (var attachment in mailData.Attachments)
            {
                bodyBuilder.Attachments.Add(attachment);
            }
        }

        mailToSend.Body = bodyBuilder.ToMessageBody();

        using SmtpClient smtpClient = new();

        await smtpClient.ConnectAsync(_mailSettings.Server, _mailSettings.Port);
        //await smtpClient.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);

        await smtpClient.SendAsync(mailToSend);

        await smtpClient.DisconnectAsync(quit: true);
    }
}
