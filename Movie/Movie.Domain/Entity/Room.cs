using Shared.Entity;

namespace Movie.Domain.Entity;

public class Room : BaseEntity
{
    public string? RoomNumber { get; set; }
    public int AvailableSeat { get; set; }
    public int SingleSeat { get; set; }
    public int CoupleSeat { get; set; }
}