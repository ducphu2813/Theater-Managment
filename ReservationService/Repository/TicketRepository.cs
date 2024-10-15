using MongoDB.Driver;
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
    
    //lấy danh sách vé theo id lịch chiếu
    public async Task<List<Ticket>> GetByScheduleIdAsync(string scheduleId)
    {
        var filter = Builders<Ticket>.Filter.Eq("MovieScheduleId", scheduleId);
        return await _collection.Find(filter).ToListAsync();
    }
    
}