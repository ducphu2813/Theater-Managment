using MongoDB.Driver;
using Reservation.Application.Interfaces.Service;
using Reservation.Domain.Entity;
using Reservation.Domain.Interfaces;

namespace Reservation.Application.Service;


public class SeatService : ISeatService
{
    private readonly ISeatRepository _seatRepository;
    
    public SeatService(ISeatRepository seatRepository)
    {
        _seatRepository = seatRepository;
    }
    
    public async Task<Dictionary<string, object>> GetAllAsync(int page, int limit)
    {
        return await _seatRepository.GetAll(page, limit);
    }
    
    public async Task<Seat> GetByIdAsync(string id)
    {
        return await _seatRepository.GetById(id) ?? throw new MongoException($"Seat with id {id} was not found.");
    }
    
    //lấy danh sách ghế theo số phòng
    public async Task<IEnumerable<Seat>> GetByRoomNumberAsync(string roomNumber)
    {
        return await _seatRepository.GetByRoomNumberAsync(roomNumber);
    }
    
    public async Task<Seat> AddAsync(Seat seat)
    {
        return await _seatRepository.Add(seat);
    }
    
    //hàm thêm danh sách nhiều ghế
    public async Task<List<Seat>> AddListAsync(List<Seat> seats)
    {
        return await _seatRepository.AddListAsync(seats);
    }
    
    public async Task<Seat> UpdateAsync(string id, Seat seat)
    {
        Seat seatToUpdate = await _seatRepository.GetById(id);
        
        if(seatToUpdate == null)
        {
            throw new MongoException($"Seat with id {id} was not found.");
        }
        
        return await _seatRepository.Update(id, seat);
    }
    
    public async Task<bool> RemoveAsync(string id)
    {
        Seat seatToRemove = await _seatRepository.GetById(id);
        
        if(seatToRemove == null)
        {
            throw new MongoException($"Seat with id {id} was not found.");
        }
        
        return await _seatRepository.Remove(id);
    }
    
    //update nhiều seat
    public async Task<List<Seat>> UpdateListAsync(List<Seat> seats)
    {
        return await _seatRepository.UpdateListAsync(seats);
    }
    
}