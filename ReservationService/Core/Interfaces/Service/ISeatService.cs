
using ReservationService.Core.Entity.Model;

namespace ReservationService.Core.Interfaces.Service;

public interface ISeatService
{
    Task<IEnumerable<Seat>> GetAllAsync();
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
}