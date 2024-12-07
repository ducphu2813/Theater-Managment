using Shared.Entity;

namespace Reservation.Domain.Entity;

public class Seat : BaseEntity
{
    public string? RoomNumber { get; set; }
    public string? Row { get; set; }
    public string? Column { get; set; }
    public string? SeatType { get; set; }
}