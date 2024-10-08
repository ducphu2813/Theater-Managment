﻿using ReservationService.Entity.Model;

namespace ReservationService.Service.Interface;

public interface IFoodService
{
    Task<IEnumerable<Food>> GetAllAsync();
    Task<Food> GetByIdAsync(string id);
    Task<Food> AddAsync(Food food);
    Task<Food> UpdateAsync(string id, Food food);
    Task<bool> RemoveAsync(string id);
    
}