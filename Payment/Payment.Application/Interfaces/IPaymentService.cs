namespace Payment.Application.Interfaces;


public interface IPaymentService
{
    Task<Dictionary<string, object>> GetAllAsync(int page, int limit);
    Task<Domain.Entity.Payment> GetPaymentByIdAsync(string id);
    Task<Domain.Entity.Payment> AddPaymentAsync(Domain.Entity.Payment payment);
    Task<Domain.Entity.Payment> UpdatePaymentAsync(string id, Domain.Entity.Payment payment);
    Task<bool> RemovePaymentAsync(string id);
    
    //hàm gửi thông tin payment đến queue
    Task<Object> UpdateTicketStatus(string ticketId, string Status, string PaymentId, string paymentMethod);
    
    //tìm payment theo ticket id
    Task<Domain.Entity.Payment?> GetByTicketIdAsync(string ticketId);
    
    //tìm bằng payment id
    Task<Domain.Entity.Payment> GetByPaymentIdAsync(string paymentId);
    
    //xóa tất cả payment
    Task<bool> RemoveAll();
}