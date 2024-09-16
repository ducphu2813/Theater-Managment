using PaymentService.Entity.Model;
using PaymentService.Repository.MongoDBRepo;

namespace PaymentService.Repository.Interface;

public interface IPaymentRepository : IRepository<Payment>
{
    
}