namespace ReservationService.Core.DTO;

public class DiscountDetailDTO
{
    public string? FoodType { get; set; }
    public string? SeatType { get; set; }
    public int PercentOff { get; set; }
}