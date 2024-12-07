using Reservation.Domain.DTO;
using Reservation.Domain.Entity;

namespace Reservation.Application.Interfaces.Service;


public interface ITicketService
{
    Task<Dictionary<string, object>> GetAllAsync(int page, int limit);
    Task<Dictionary<String, Object>> GetByIdAsync(string id);
    Task<Ticket> AddAsync(SaveTicketDTO ticket);
    Task<Ticket> UpdateAsync(string id, UpdateTicketDTO ticket);
    Task<bool> RemoveAsync(string id);
    
    //lấy vé theo user id
    Task<List<Ticket>> GetByUserIdAsync(string userId);
    
    //hàm xóa tất cả vé
    Task<bool> RemoveAllAsync();
    
    //lấy danh sách vé theo id lịch chiếu
    Task<List<Ticket>> GetByScheduleIdAsync(string scheduleId);
    
    //lấy tất cả SeatDetail theo id lịch chiếu
    Task<Dictionary<String, Object>> GetAllBookedSeatByScheduleIdAsync(string scheduleId, string roomNumber);
    
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