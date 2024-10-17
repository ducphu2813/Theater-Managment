using ReservationService.Entity.Model;
using ReservationService.Repository.MongoDBRepo;

namespace ReservationService.Repository.Interface;

public interface ISeatRepository : IRepository<Seat>
{
    //hàm lấy danh sách Seat theo RoomNumber
    Task<List<Seat>> GetByRoomNumberAsync(string roomNumber);
    
    //hàm lấy danh sách Seat theo danh sách id
    Task<List<Seat>> GetByIdsAsync(List<string> ids);
}