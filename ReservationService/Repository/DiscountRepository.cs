using ReservationService.Context;
using ReservationService.Entity.Model;
using ReservationService.Repository.Interface;
using ReservationService.Repository.MongoDBRepo;

namespace ReservationService.Repository;

public class DiscountRepository : MongoDBRepository<Discount>, IDiscountRepository
{
    public DiscountRepository(MongoDBContext context) : base(context, "Discount")
    {
        
    }
    
}