using MongoDB.Driver;
using Payment.Domain.Interfaces;
using Shared.Context;
using Shared.Repository;

namespace Payment.Infrastructure.Repository;


public class PaymentRepository : MongoDBRepository<Domain.Entity.Payment>, IPaymentRepository
{
    public PaymentRepository(MongoDBContext context) : base(context, "Payment")
    {
    }
    
    //hàm tìm bằng Ticket Id
    public async Task<Domain.Entity.Payment?> GetByTicketIdAsync(string ticketId)
    {
        var filter = Builders<Domain.Entity.Payment>.Filter.Eq("TicketId", ticketId);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
    
    //tìm bằng payment id
    public async Task<Domain.Entity.Payment> GetByPaymentIdAsync(string paymentId)
    {
        var filter = Builders<Domain.Entity.Payment>.Filter.Eq("PaymentId", paymentId);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
    
    //hàm xóa tất cả payment
    public async Task<bool> RemoveAll()
    {
        await _collection.DeleteManyAsync(Builders<Domain.Entity.Payment>.Filter.Empty);
        return true;
    }
    
}