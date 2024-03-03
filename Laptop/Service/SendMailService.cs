using Laptop.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Laptop.Service
{
    public class SendMailService
    {
        MailSetting _mailSettings { get; set; }

        public SendMailService(IOptions<MailSetting> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendMail(MailContent mailContent)
        {
            
                var email = new MimeMessage();

                email.Sender = new MailboxAddress(_mailSettings.DisplayName,_mailSettings.Mail);

                email.From.Add(new MailboxAddress(_mailSettings.DisplayName,_mailSettings.Mail));
                email.To.Add(new MailboxAddress(mailContent.To,mailContent.To));

                email.Subject = mailContent.Subject;

                var builder = new BodyBuilder();

                builder.TextBody = mailContent.Body;

                email.Body = builder.ToMessageBody();

                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                }
            }
          
            
        }
    }
    public class MailContent
    {
        public string To { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }

        public int Port { get; set; }
    }

