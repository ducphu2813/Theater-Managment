using Shared.Interfaces;

namespace Payment.Domain.Interfaces;

public interface IPaymentRepository : IRepository<Entity.Payment>
{
    //hàm tìm bằng Ticket Id
    Task<Entity.Payment?> GetByTicketIdAsync(string ticketId);
    
    //tìm bằng payment id
    Task<Entity.Payment> GetByPaymentIdAsync(string paymentId);
    
    //hàm xóa tất cả payment
    Task<bool> RemoveAll();
}