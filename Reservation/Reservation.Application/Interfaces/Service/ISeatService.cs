using Reservation.Domain.Entity;

namespace Reservation.Application.Interfaces.Service;


public interface ISeatService
{
    Task<Dictionary<string, object>> GetAllAsync(int page, int limit);
    Task<Seat> GetByIdAsync(string id);
    Task<Seat> AddAsync(Seat seat);
    Task<Seat> UpdateAsync(string id, Seat seat);
    Task<bool> RemoveAsync(string id);
    
    //hàm lấy danh sách Seat theo RoomNumber
    Task<IEnumerable<Seat>> GetByRoomNumberAsync(string roomNumber);
    
    //hàm thêm danh sách nhiều ghế
    Task<List<Seat>> AddListAsync(List<Seat> seats);
    
    //update nhiều seat
    Task<List<Seat>> UpdateListAsync(List<Seat> seats);
    
    //xóa ghế theo số phòng
    Task<bool> RemoveByRoomNumberAsync(string roomNumber);
}