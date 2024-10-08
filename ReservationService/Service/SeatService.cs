﻿using ReservationService.Entity.Model;
using ReservationService.Exceptions;
using ReservationService.Repository.Interface;
using ReservationService.Service.Interface;

namespace ReservationService.Service;

public class SeatService : ISeatService
{
    private readonly ISeatRepository _seatRepository;
    
    public SeatService(ISeatRepository seatRepository)
    {
        _seatRepository = seatRepository;
    }
    
    public async Task<IEnumerable<Seat>> GetAllAsync()
    {
        return await _seatRepository.GetAll();
    }
    
    public async Task<Seat> GetByIdAsync(string id)
    {
        return await _seatRepository.GetById(id) ?? throw new NotFoundException($"Seat with id {id} was not found.");
    }
    
    public async Task<Seat> AddAsync(Seat seat)
    {
        return await _seatRepository.Add(seat);
    }
    
    public async Task<Seat> UpdateAsync(string id, Seat seat)
    {
        Seat seatToUpdate = await _seatRepository.GetById(id);
        
        if(seatToUpdate == null)
        {
            throw new NotFoundException($"Seat with id {id} was not found.");
        }
        
        return await _seatRepository.Update(id, seat);
    }
    
    public async Task<bool> RemoveAsync(string id)
    {
        Seat seatToRemove = await _seatRepository.GetById(id);
        
        if(seatToRemove == null)
        {
            throw new NotFoundException($"Seat with id {id} was not found.");
        }
        
        return await _seatRepository.Remove(id);
    }
    
}