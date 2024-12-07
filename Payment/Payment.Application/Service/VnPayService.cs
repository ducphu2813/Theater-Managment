﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Payment.Application.Interfaces;
using Payment.Application.Libraries;
using Payment.Domain.Entity;

namespace Payment.Application.Service;


public class VnPayService : IVnPayService
{
    
    private readonly IConfiguration _configuration;
    private readonly IActionContextAccessor _actionContextAccessor;
    
    public VnPayService(IConfiguration configuration,
                        IActionContextAccessor actionContextAccessor) {
        _configuration = configuration;
        _actionContextAccessor = actionContextAccessor;
    }
    
    public string CreatePaymentUrl(PaymentInformationModel model, HttpContext context, DateTime expireDate)
    {
        var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
        var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);

        var expiryTime = TimeZoneInfo.ConvertTimeFromUtc(expireDate, timeZoneById);
        
        var urlCallBack = "http://localhost:5173/payment/payment_result";
        
        var pay = new VnPayLibrary();
        pay.AddRequestData("vnp_Version", _configuration["VnPay:Version"]);
        pay.AddRequestData("vnp_Command", _configuration["VnPay:Command"]);
        pay.AddRequestData("vnp_TmnCode", _configuration["VnPay:TmnCode"]);
        pay.AddRequestData("vnp_Locale", _configuration["VnPay:Locale"]);
        pay.AddRequestData("vnp_CurrCode", _configuration["VnPay:CurrCode"]);
        pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
        pay.AddRequestData("vnp_TxnRef", timeNow.Ticks.ToString());
        pay.AddRequestData("vnp_OrderInfo", model.TicketID);
        pay.AddRequestData("vnp_OrderType", model.OrderType);
        pay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString());
        pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
        pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
        
        // Truyền thời gian hết hạn vào tham số vnp_ExpireDate
        pay.AddRequestData("vnp_ExpireDate", expiryTime.ToString("yyyyMMddHHmmss"));
        
        var paymentUrl = 
            pay.CreateRequestUrl(_configuration["VnPay:Url"], _configuration["VnPay:HashSecret"]);
        
        return paymentUrl;
    }

    public PaymentResponseModel PaymentExecute(IQueryCollection collections)
    {
        var pay = new VnPayLibrary();
        var response = 
            pay.GetFullResponseData(collections, _configuration["VnPay:HashSecret"]);

        return response;
    }


    public PaymentResponseModel GetFullResponseData(IQueryCollection collection, string hashSecret)
    {
        var vnPay = new VnPayLibrary();

        foreach (var (key, value) in collection)
        {
            if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
            {
                vnPay.AddResponseData(key, value);
            }
        }
        
        var orderId = Convert.ToString(vnPay.GetResponseData("vnp_TxnRef"));
        var vnPayTranId = Convert.ToString(vnPay.GetResponseData("vnp_TransactionNo"));
        var vnpResponseCode = vnPay.GetResponseData("vnp_ResponseCode");
        var vnpSecureHash =
            collection.FirstOrDefault(k => k.Key == "vnp_SecureHash").Value; //hash của dữ liệu trả về
        var orderInfo = vnPay.GetResponseData("vnp_OrderInfo");
        DateTime date;
        if (DateTime.TryParseExact(vnPay.GetResponseData("vnp_PayDate"), "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out date))
        {
            Console.WriteLine("Date: " + date);
        }
        else
        {
            Console.WriteLine("Invalid date format.");
        }
        var checkSignature =
            vnPay.ValidateSignature(vnpSecureHash, hashSecret); //check Signature

        if (!checkSignature)
            return new PaymentResponseModel()
            {
                Success = false
            };

        return new PaymentResponseModel()
        {
            Success = true,
            PaymentMethod = "VnPay",
            OrderDescription = orderInfo,
            OrderId = orderInfo,
            PaymentId = vnPayTranId.ToString(),
            TransactionId = vnPayTranId.ToString(),
            Token = vnpSecureHash,
            VnPayResponseCode = vnpResponseCode,
            Amount = Convert.ToInt64(vnPay.GetResponseData("vnp_Amount")) / 100,
            CreatedAt = date
        };
    }
}