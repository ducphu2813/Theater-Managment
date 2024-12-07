using MongoDB.Driver;
using Reservation.Application.Interfaces.Service;
using Reservation.Domain.Entity;
using Reservation.Domain.Interfaces;

namespace Reservation.Application.Service;


public class FoodService : IFoodService
{
    private readonly IFoodRepository _foodRepository;

    public FoodService(IFoodRepository foodRepository)
    {
        _foodRepository = foodRepository;
    }

    public async Task<Dictionary<string, object>> GetAllAsync(int page, int limit)
    {
        return await _foodRepository.GetAll(page, limit);
    }

    public async Task<Food> GetByIdAsync(string id)
    {
        return await _foodRepository.GetById(id) ?? throw new MongoException($"Food with id {id} was not found.");
    }

    public async Task<Food> AddAsync(Food food)
    {
        return await _foodRepository.Add(food);
    }

    public async Task<Food> UpdateAsync(string id, Food food)
    {
        Food foodToUpdate = await _foodRepository.GetById(id);
        
        if(foodToUpdate == null)
        {
            throw new MongoException($"Food with id {id} was not found.");
        }
        
        return await _foodRepository.Update(id, food);
    }

    public async Task<bool> RemoveAsync(string id)
    {
        Food foodToRemove = await _foodRepository.GetById(id);
        
        if(foodToRemove == null)
        {
            throw new MongoException($"Food with id {id} was not found.");
        }
        
        return await _foodRepository.Remove(id);
    }
    
    //hàm tìm food nâng cao
    public async Task<Dictionary<string, object>> GetAllAdvance(
        int page
        , int limit
        , List<string> foodType)
    {
        return await _foodRepository.GetAllAdvance(page, limit, foodType);
    }
    
}