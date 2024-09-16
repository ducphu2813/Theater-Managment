using PaymentService.Entity.Model;
using PaymentService.Exceptions;
using PaymentService.Repository.Interface;
using PaymentService.Service.Interface;

namespace PaymentService.Service;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    
    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
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
        
        return payment;
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
    
    public async Task<bool> RemovePaymentAsync(string id)
    {
        var existingPayment = await _paymentRepository.GetById(id);
        
        if (existingPayment == null)
        {
            throw new NotFoundException($"Payment with id {id} was not found.");
        }
        
        return await _paymentRepository.Remove(id);
    }
    
}