using Laptop.Interface;
using System.Net;
using System.Net.Mail;

namespace Laptop.Repository
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string sender, string email, string subject, string msg) 
        {
           
            var mail = sender;
            var pw = "rptl camk yrmc tjeh";

            var client = new SmtpClient("smtp.gmail.com",587)
            {   
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mail, pw)
            };
            return client.SendMailAsync(
                new MailMessage(from: mail,
                                to: email,
                                subject,
                                msg));

        }
    }
}
