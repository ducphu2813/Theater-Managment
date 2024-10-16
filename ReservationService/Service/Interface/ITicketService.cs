﻿using ReservationService.DTO;
using ReservationService.Entity.Model;

namespace ReservationService.Service.Interface;

public interface ITicketService
{
    Task<IEnumerable<Ticket>> GetAllAsync();
    Task<Dictionary<String, Object>> GetByIdAsync(string id);
    Task<Ticket> AddAsync(SaveTicketDTO ticket);
    Task<Ticket> UpdateAsync(string id, UpdateTicketDTO ticket);
    Task<bool> RemoveAsync(string id);
    
    //hàm xóa tất cả vé
    Task<bool> RemoveAllAsync();
    
    //lấy danh sách vé theo id lịch chiếu
    Task<List<Ticket>> GetByScheduleIdAsync(string scheduleId);
    
    //lấy tất cả SeatDetail theo id lịch chiếu
    Task<Dictionary<String, Object>> GetAllBookedSeatByScheduleIdAsync(string scheduleId, string roomNumber);
}