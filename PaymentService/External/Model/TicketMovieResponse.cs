namespace PaymentService.External.Model;

public class TicketMovieResponse
{
    public Ticket Ticket { get; set; }
    public MovieSchedule MovieSchedule { get; set; }
}

public class Ticket
{
    public string? Id { get; set; }
    
    public string? MovieScheduleId { get; set; }
    public List<string>? SeatId { get; set; }
    public List<Seat>? SeatDetail  { get; set; }
    public List<string>? FoodId { get; set; }
    public List<Food>? FoodDetail { get; set; }
    public float? BaseAmount { get; set; }
    public float? TotalAmount { get; set; }
    public string? DiscountId { get; set; }
    public Discount? DiscountDetail { get; set; }
    public string? UserId { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; } 
    public DateTime? ExpiryTime { get; set; }
}

public class Seat
{
    public string? Id { get; set; }

    public string? RoomNumber { get; set; }
    public string? Row { get; set; }
    public string? Column { get; set; }
    public string? SeatType { get; set; }
}

public class Food
{
    public string? Id { get; set; }
    
    public string? Name { get; set; }
    public string? FoodType { get; set; }
    public string? Description { get; set; }
    public int? Amount { get; set; }
}

public class Discount
{
    public string? Id { get; set; }
    
    public string? FoodType { get; set; }
    public string? SeatType { get; set; }
    public int? PercentOff { get; set; }
}

public class MovieSchedule
{
    public string? Id { get; set; }
    public movies? Movie  { get; set; }
    public string? RoomNumber { get; set; }
    public DateTime? ShowTime { get; set; }
    public int? SingleSeatPrice { get; set; }
    public int? CoupleSeatPrice { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Status { get; set; }
}

public class movies
{
    public string Id { get; set; }
    
    public required string Name { get; set; }
    public string? Director { get; set; }
    public string? Actors { get; set; }
    public string? Author { get; set; }
    public string? Description { get; set; }
    public string? Dub { get; set; }
    public string? SubTitle { get; set; }
}