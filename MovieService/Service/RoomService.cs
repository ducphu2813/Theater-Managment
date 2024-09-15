using MovieService.Entity.Model;
using MovieService.Repository.Interface;
using MovieService.Service.Interface;

namespace MovieService.Service;

public class RoomService : IRoomService
{
    
    private readonly IRoomRepository _roomRepository;
    
    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }
    
    public async Task<IEnumerable<Room>> GetAllAsync()
    {
        return await _roomRepository.GetAll();
    }

    public async Task<Room> GetByIdAsync(string id)
    {
        return await _roomRepository.GetById(id);
    }

    public async Task<Room> AddAsync(Room room)
    {
        return await _roomRepository.Add(room);
    }
    
    public async Task<IEnumerable<Room>> AddListAsync(List<Room> rooms)
    {
        return await _roomRepository.AddList(rooms);
    }

    public async Task<Room> UpdateAsync(string id, Room room)
    {
        return await _roomRepository.Update(id, room);
    }

    public async Task<bool> RemoveAsync(string id)
    {
        return await _roomRepository.Remove(id);
    }
}