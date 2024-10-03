using ReservationService.Entity.Model;
using ReservationService.Repository.MongoDBRepo;

namespace ReservationService.Repository.Interface;

public interface ITicketRepository : IRepository<Ticket>
{
    //lấy danh sách vé theo id lịch chiếu
    Task<List<Ticket>> GetByScheduleIdAsync(string scheduleId);
}