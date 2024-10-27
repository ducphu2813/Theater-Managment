using PaymentService.Entity.Model;
using PaymentService.Repository.MongoDBRepo;

namespace PaymentService.Repository.Interface;

public interface IPaymentRepository : IRepository<Payment>
{
    //hàm tìm bằng Ticket Id
    Task<Payment?> GetByTicketIdAsync(string ticketId);
    
    //hàm xóa tất cả payment
    Task<bool> RemoveAll();
}