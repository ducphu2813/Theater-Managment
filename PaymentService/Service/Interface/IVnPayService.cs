using PaymentService.Entity.Model;

namespace PaymentService.Service.Interface;

public interface IVnPayService
{
    string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
    PaymentResponseModel PaymentExecute(IQueryCollection collections);
    PaymentResponseModel GetFullResponseData(IQueryCollection collection, string hashSecret);
}