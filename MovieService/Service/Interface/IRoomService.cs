using MovieService.Entity.Model;

namespace MovieService.Service.Interface;

public interface IRoomService
{
    Task<IEnumerable<Room>> GetAllAsync();
    Task<Room> GetByIdAsync(string id);
    Task<Room> AddAsync(Room room);
    Task<IEnumerable<Room>> AddListAsync(List<Room> rooms);
    Task<Room> UpdateAsync(string id, Room room);
    Task<bool> RemoveAsync(string id);
    
    
}