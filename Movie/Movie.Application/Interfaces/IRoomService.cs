using Movie.Domain.Entity;

namespace Movie.Application.Interfaces;

public interface IRoomService
{
    Task<Dictionary<string, object>> GetAllAsync(int page, int limit);
    Task<Room> GetByIdAsync(string id);
    Task<Room> AddAsync(Room room);
    Task<IEnumerable<Room>> AddListAsync(List<Room> rooms);
    Task<Room> UpdateAsync(string id, Room room);
    Task<bool> RemoveAsync(string id);
    
    
}