using Laptop.Models;
using Laptop.Models.Momo;

namespace Laptop.Service
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(long total);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    }
}
