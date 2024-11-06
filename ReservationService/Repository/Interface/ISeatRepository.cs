using ReservationService.Entity.Model;
using ReservationService.Repository.MongoDBRepo;

namespace ReservationService.Repository.Interface;

public interface ISeatRepository : IRepository<Seat>
{
    //hàm lấy danh sách Seat theo RoomNumber
    Task<List<Seat>> GetByRoomNumberAsync(string roomNumber);
    
    //hàm lấy danh sách Seat theo danh sách id
    Task<List<Seat>> GetByIdsAsync(List<string> ids);
    
    //thêm danh sách nhiều ghế
    Task<List<Seat>> AddListAsync(List<Seat> seats);
    
    //update nhiều seat
    Task<List<Seat>> UpdateListAsync(List<Seat> seats);
}