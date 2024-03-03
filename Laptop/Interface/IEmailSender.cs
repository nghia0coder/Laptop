namespace Laptop.Interface
{
    public interface IEmailSender
    {
        Task SendEmailAsync (string sender,string email,string subject, string msg);
    }
}
