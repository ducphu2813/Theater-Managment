using ReservationService.Entity.Model;
using ReservationService.Repository.Interface;
using ReservationService.Repository.MongoDBRepo;
using ReservationService.Context;

namespace ReservationService.Repository;

public class SeatRepository : MongoDBRepository<Seat>, ISeatRepository
{
    public SeatRepository(MongoDBContext context) : base(context, "Seat")
    {
    }
    
}