using SimpleSelfEmployApi.Dtos;
using SimpleSelfEmployApi.Models;

namespace SimpleSelfEmployApi.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDto>> Index(int? pageNumber, string? jobId, int limit);
        Task<PaymentDto> SavePayment(PaymentDto Payment);
        Task<PaymentDto> GetPayment(string id);
        Task<bool> DeletePayment(string id);
    }
}
