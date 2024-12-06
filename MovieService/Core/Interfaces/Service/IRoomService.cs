using MovieService.Core.Entity.Model;

namespace MovieService.Core.Interfaces.Service;

public interface IRoomService
{
    Task<Dictionary<string, object>> GetAllAsync(int page, int limit);
    Task<Room> GetByIdAsync(string id);
    Task<Room> AddAsync(Room room);
    Task<IEnumerable<Room>> AddListAsync(List<Room> rooms);
    Task<Room> UpdateAsync(string id, Room room);
    Task<bool> RemoveAsync(string id);
    
    
}