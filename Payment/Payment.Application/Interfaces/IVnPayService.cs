using Microsoft.AspNetCore.Http;
using Payment.Domain.Entity;

namespace Payment.Application.Interfaces;

public interface IVnPayService
{
    string CreatePaymentUrl(PaymentInformationModel model, HttpContext context, DateTime expireDate);
    PaymentResponseModel PaymentExecute(IQueryCollection collections);
    PaymentResponseModel GetFullResponseData(IQueryCollection collection, string hashSecret);
}