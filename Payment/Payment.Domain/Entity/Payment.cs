using Shared.Entity;

namespace Payment.Domain.Entity;

public class Payment : BaseEntity
{

    public string? PaymentId { get; set; }
    public string? TicketId { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Status { get; set; }
    public long? Amount { get; set; }
    public DateTime? CreatedAt { get; set; }
    
}