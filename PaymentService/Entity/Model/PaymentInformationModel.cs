namespace PaymentService.Entity.Model;

public class PaymentInformationModel
{
    public string? OrderType { get; set; }
    public string? TicketID { get; set; }
    public double? Amount { get; set; }
    public string? OrderDescription { get; set; }
    public string? Name { get; set; }
    public string? UserId { get; set; }
}