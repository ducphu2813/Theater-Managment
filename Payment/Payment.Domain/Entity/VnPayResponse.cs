﻿namespace Payment.Domain.Entity;

public class VnPayResponse
{
    public string? vnp_Amount { get; set; }
    public string? vnp_BankCode { get; set; }
    public string? vnp_BankTranNo { get; set; }
    public string? vnp_CardType { get; set; }
    public string? vnp_OrderInfo { get; set; }
    public string? vnp_PayDate { get; set; }  // cái này sẽ phải đổi thành DateTime
    public string? vnp_ResponseCode { get; set; }
    public string? vnp_TransactionNo { get; set; }
    public string? vnp_TransactionStatus { get; set; }
    public string? vnp_TxnRef { get; set; }
}