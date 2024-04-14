using Laptop.Models;
using Laptop.Models.Momo;

namespace Laptop.Service
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(int total);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    }
}
