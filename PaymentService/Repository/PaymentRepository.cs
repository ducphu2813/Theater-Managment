using MongoDB.Driver;
using PaymentService.Context;
using PaymentService.Entity.Model;
using PaymentService.Repository.Interface;
using PaymentService.Repository.MongoDBRepo;

namespace PaymentService.Repository;

public class PaymentRepository : MongoDBRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(MongoDBContext context) : base(context, "Payment")
    {
    }
    
    //hàm tìm bằng Ticket Id
    public async Task<Payment?> GetByTicketIdAsync(string ticketId)
    {
        var filter = Builders<Payment>.Filter.Eq("TicketId", ticketId);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
    
    //hàm xóa tất cả payment
    public async Task<bool> RemoveAll()
    {
        await _collection.DeleteManyAsync(Builders<Payment>.Filter.Empty);
        return true;
    }
    
}