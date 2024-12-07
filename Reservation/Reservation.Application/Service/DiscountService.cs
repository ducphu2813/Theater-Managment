using MongoDB.Driver;
using Reservation.Application.Interfaces.Service;
using Reservation.Domain.Entity;
using Reservation.Domain.Interfaces;

namespace Reservation.Application.Service;


public class DiscountService : IDiscountService
{
    private readonly IDiscountRepository _discountRepository;

    public DiscountService(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public async Task<Dictionary<string, object>> GetAllAsync(int page, int limit)
    {
        return await _discountRepository.GetAll(page, limit);
    }

    public async Task<Discount> GetByIdAsync(string id)
    {
        return await _discountRepository.GetById(id) ?? throw new MongoException($"Discount with id {id} was not found.");
    }
    
    //hàm tìm nâng cao
    public async Task<Dictionary<string, object>> GetAllAdvance(
        int page
        , int limit)
    {
        return await _discountRepository.GetAllAdvance(page, limit);
    }

    public async Task<Discount> AddAsync(Discount discount)
    {
        return await _discountRepository.Add(discount);
    }

    public async Task<Discount> UpdateAsync(string id, Discount discount)
    {
        Discount discountToUpdate = await _discountRepository.GetById(id);
        
        if(discountToUpdate == null)
        {
            throw new MongoException($"Discount with id {id} was not found.");
        }
        
        return await _discountRepository.Update(id, discount);
    }

    public async Task<bool> RemoveAsync(string id)
    {
        Discount discountToRemove = await _discountRepository.GetById(id);
        
        if(discountToRemove == null)
        {
            throw new MongoException($"Discount with id {id} was not found.");
        }
        
        return await _discountRepository.Remove(id);
    }
}