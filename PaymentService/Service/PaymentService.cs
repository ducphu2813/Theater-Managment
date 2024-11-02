using PaymentService.Entity.Model;
using PaymentService.Events;
using PaymentService.Exceptions;
using PaymentService.Messaging.Interface;
using PaymentService.Repository.Interface;
using PaymentService.Service.Interface;

namespace PaymentService.Service;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPublisher<PaymentEvent> _publisher;
    
    public PaymentService(IPaymentRepository paymentRepository,
        IPublisher<PaymentEvent> publisher)
    {
        _paymentRepository = paymentRepository;
        _publisher = publisher;
    }
    
    public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
    {
        return await _paymentRepository.GetAll();
    }
    
    public async Task<Payment> GetPaymentByIdAsync(string id)
    {
        var payment = await _paymentRepository.GetById(id);
        
        if (payment == null)
        {
            throw new NotFoundException($"Payment with id {id} was not found.");
        }
        
        //gửi payment id đến queue
        // _publisher.Publish(new PaymentEvent
        // {
        //     PaymentId = payment.Id
        // });
        
        
        return payment;
    }
    
    //tìm bằng ticket id
    public async Task<Payment?> GetByTicketIdAsync(string ticketId)
    {
        return await _paymentRepository.GetByTicketIdAsync(ticketId);
    }
    
    public async Task<Payment> AddPaymentAsync(Payment payment)
    {
        return await _paymentRepository.Add(payment);
    }
    
    public async Task<Payment> UpdatePaymentAsync(string id, Payment payment)
    {
        var existingPayment = await _paymentRepository.GetById(id);
        
        if (existingPayment == null)
        {
            throw new NotFoundException($"Payment with id {id} was not found.");
        }
        
        return await _paymentRepository.Update(id, payment);
    }

    public async Task<Object> UpdateTicketStatus(string ticketId, string Status, string PaymentId, string paymentMethod)
    {
        //gửi 3 thông tin trên đến queue
        _publisher.Publish(new PaymentEvent
        {
            PaymentId = PaymentId,
            TicketId = ticketId,
            Status = Status,
            PaymentMethod = paymentMethod
        });
        
        return new
        {
            TicketId = ticketId,
            Status = Status,
            PaymentId = PaymentId,
            PaymentMethod = paymentMethod
        };
    }
    
    public async Task<bool> RemovePaymentAsync(string id)
    {
        var existingPayment = await _paymentRepository.GetById(id);
        
        if (existingPayment == null)
        {
            throw new NotFoundException($"Payment with id {id} was not found.");
        }
        
        return await _paymentRepository.Remove(id);
    }
    
    //tìm bằng payment id
    public async Task<Payment> GetByPaymentIdAsync(string paymentId)
    {
        return await _paymentRepository.GetByPaymentIdAsync(paymentId);
    }
    
    //xóa tất cả payment
    public async Task<bool> RemoveAll()
    {
        return await _paymentRepository.RemoveAll();
    }
    
}