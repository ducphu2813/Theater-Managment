using ReservationService.Context;
using ReservationService.Entity.Model;
using ReservationService.Repository.Interface;
using ReservationService.Repository.MongoDBRepo;

namespace ReservationService.Repository;

public class FoodRepository : MongoDBRepository<Food>, IFoodRepository
{
    public FoodRepository(MongoDBContext context) : base(context, "Food")
    {
    }
    
}