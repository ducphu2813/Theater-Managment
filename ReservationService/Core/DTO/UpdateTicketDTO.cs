namespace ReservationService.Core.DTO;

public class UpdateTicketDTO
{
    public string? MovieScheduleId { get; set; }
    public List<string>? SeatId { get; set; }
    public List<string>? FoodId { get; set; }
    public string? DiscountId { get; set; }
    public int TotalTicket { get; set; }
    public int TotalPrice { get; set; }
    public string? UserId { get; set; }
}