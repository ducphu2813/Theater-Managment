
using ReservationService.Core.Entity.Model;

namespace ReservationService.Core.Interfaces.Repository;

public interface ITicketRepository : IRepository<Ticket>
{
    //lấy danh sách vé theo id lịch chiếu
    Task<List<Ticket>> GetByScheduleIdAsync(string scheduleId);
    
    //tìm vé theo user id
    Task<List<Ticket>> GetByUserIdAsync(string userId);
    
    //hàm xóa tất cả vé
    Task<bool> RemoveAllAsync();
    
    //hàm tìm bằng schelude id và seat id
    Task<List<Ticket>> GetByScheduleIdAndSeatIdAsync(string scheduleId, List<string> seatIds);
    
    //hàm tìm ticket nâng cao
    Task<Dictionary<string, object>> GetAllAdvance(
        int page
        , int limit
        , string userId
        , List<string> scheduleId
        , string status
        , DateTime fromCreateDate
        , DateTime toCreateDate
        , float fromTotalPrice
        , float toTotalPrice
        , string sortByCreateDate
        , string sortByTotalPrice);
}