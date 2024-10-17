using ReservationService.Entity.Model;
using ReservationService.Repository.MongoDBRepo;

namespace ReservationService.Repository.Interface;

public interface IDiscountRepository : IRepository<Discount>
{
    //hàm lấy danh sách Discount theo FoodType và SeatType
    Task<List<Discount>> GetByFoodTypeAndSeatTypeAsync(List<string> foodTypes, List<string> seatTypes);
}