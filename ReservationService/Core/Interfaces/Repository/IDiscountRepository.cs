
using ReservationService.Core.Entity.Model;
using ReservationService.Core.Interfaces.Repository;

namespace ReservationService.Core.Interfaces.Repository;

public interface IDiscountRepository : IRepository<Discount>
{
    //hàm lấy danh sách Discount theo FoodType và SeatType
    Task<List<Discount>> GetByFoodTypeAndSeatTypeAsync(List<string> foodTypes, List<string> seatTypes);
    
    //hàm tìm nâng cao
    Task<Dictionary<string, object>> GetAllAdvance(
        int page
        , int limit);
}