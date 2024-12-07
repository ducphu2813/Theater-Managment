using Reservation.Domain.Entity;
using Shared.Interfaces;

namespace Reservation.Domain.Interfaces;

public interface IDiscountRepository : IRepository<Discount>
{
    //hàm lấy danh sách Discount theo FoodType và SeatType
    Task<List<Discount>> GetByFoodTypeAndSeatTypeAsync(List<string> foodTypes, List<string> seatTypes);
    
    //hàm tìm nâng cao
    Task<Dictionary<string, object>> GetAllAdvance(
        int page
        , int limit);
}