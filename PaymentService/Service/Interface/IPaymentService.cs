using PaymentService.Entity.Model;

namespace PaymentService.Service.Interface;

public interface IPaymentService
{
    Task<IEnumerable<Payment>> GetAllPaymentsAsync();
    Task<Payment> GetPaymentByIdAsync(string id);
    Task<Payment> AddPaymentAsync(Payment payment);
    Task<Payment> UpdatePaymentAsync(string id, Payment payment);
    Task<bool> RemovePaymentAsync(string id);
}