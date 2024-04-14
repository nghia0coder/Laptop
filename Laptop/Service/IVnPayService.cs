using Laptop.Models;

namespace Laptop.Service
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(long total, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
