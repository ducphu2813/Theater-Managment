using ReservationService.Context;
using ReservationService.Entity.Model;
using ReservationService.Repository.Interface;
using ReservationService.Repository.MongoDBRepo;

namespace ReservationService.Repository;

public class TicketRepository : MongoDBRepository<Ticket>, ITicketRepository
{
    public TicketRepository(MongoDBContext context) : base(context, "Ticket")
    {
    }
    
}