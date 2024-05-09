namespace Laptop.Interface
{
    public interface IEmailSender
    {
        Task SendEmail(string sender,string email,string subject, string msg);

        Task SendEmailAsync(string email, string subject, string message);

        Task SendSmsAsync(string number, string message);
    }
}
