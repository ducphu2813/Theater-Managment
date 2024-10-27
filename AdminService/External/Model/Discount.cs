namespace AdminService.External.Model;

public class Discount
{
    public string? Id { get; set; }
    
    public string? FoodType { get; set; }
    public string? SeatType { get; set; }
    public int? PercentOff { get; set; }
}