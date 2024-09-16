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
    
}