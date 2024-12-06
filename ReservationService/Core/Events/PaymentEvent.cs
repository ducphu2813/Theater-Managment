namespace ReservationService.Core.Events;

public class PaymentEvent
{
    public string? PaymentId { get; set; }
    public string? PaymentMethod { get; set; }
    public string? TicketId { get; set; }
    public string? Status { get; set; }
}