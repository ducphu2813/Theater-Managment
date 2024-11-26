using ReservationService.Entity.Model;
using ReservationService.Exceptions;
using ReservationService.Repository.Interface;
using ReservationService.Service.Interface;

namespace ReservationService.Service;

public class FoodService : IFoodService
{
    private readonly IFoodRepository _foodRepository;

    public FoodService(IFoodRepository foodRepository)
    {
        _foodRepository = foodRepository;
    }

    public async Task<IEnumerable<Food>> GetAllAsync()
    {
        return await _foodRepository.GetAll();
    }

    public async Task<Food> GetByIdAsync(string id)
    {
        return await _foodRepository.GetById(id) ?? throw new NotFoundException($"Food with id {id} was not found.");
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
            throw new NotFoundException($"Food with id {id} was not found.");
        }
        
        return await _foodRepository.Update(id, food);
    }

    public async Task<bool> RemoveAsync(string id)
    {
        Food foodToRemove = await _foodRepository.GetById(id);
        
        if(foodToRemove == null)
        {
            throw new NotFoundException($"Food with id {id} was not found.");
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