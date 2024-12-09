using Movie.Domain.Entity;
using Shared.Interfaces;

namespace Movie.Domain.Interface;

public interface IRoomRepository : IRepository<Room>
{

    Task<IEnumerable<Room>> AddList(List<Room> rooms);
    
    //hàm kiểm tra xem phòng có tồn tại ko theo số phòng
    Task<bool> CheckExistRoomAsync(string roomNumber);

}