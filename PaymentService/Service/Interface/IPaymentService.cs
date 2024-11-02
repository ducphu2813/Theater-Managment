using PaymentService.Entity.Model;

namespace PaymentService.Service.Interface;

public interface IPaymentService
{
    Task<IEnumerable<Payment>> GetAllPaymentsAsync();
    Task<Payment> GetPaymentByIdAsync(string id);
    Task<Payment> AddPaymentAsync(Payment payment);
    Task<Payment> UpdatePaymentAsync(string id, Payment payment);
    Task<bool> RemovePaymentAsync(string id);
    
    //hàm gửi thông tin payment đến queue
    Task<Object> UpdateTicketStatus(string ticketId, string Status, string PaymentId, string paymentMethod);
    
    //tìm payment theo ticket id
    Task<Payment?> GetByTicketIdAsync(string ticketId);
    
    //tìm bằng payment id
    Task<Payment> GetByPaymentIdAsync(string paymentId);
    
    //xóa tất cả payment
    Task<bool> RemoveAll();
}