﻿using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Payment.Application.Helper;
using Payment.Application.Interfaces;
using Payment.Application.Libraries;
using Payment.Domain.Entity;
using Payment.Domain.External;

namespace Payment.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IVnPayService _vnPayService;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    
    public PaymentController(IPaymentService paymentService,
            IVnPayService vnPayService,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
    {
        _paymentService = paymentService;
        _vnPayService = vnPayService;
        _configuration = configuration;
        _httpClient = httpClientFactory.CreateClient("reservation-service");
    }
    
    //gửi yêu cầu thanh toán(để nhận url thanh toán)
    [HttpPost]
    [Route("create_payment_url")]
    public async Task<IActionResult> CreatePaymentUrl([FromBody] PaymentInformationModel model)
    {
        
        //note cần làm: kiểm tra ticket id có tồn tại không,
        //nếu status của ticket là "processed" thì không cho thanh toán
        //và cần kiểm tra UserId có trùng với UserId trong ticket không
        //khi đã lấy được ticket thì gán Amount bằng giá vé của ticket

        //kiểm tra ticket id
        //gọi api đến reservation service lấy ticket
        var response = await _httpClient.GetAsync($"internal/TicketInternal/{model.TicketID}");
        
        //cái này để chuyển từ snake_case sang camelCase
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        //chuyển thành object để dễ xử lý
        var ticketContent = await response.Content.ReadAsStringAsync();
        var ticket = JsonSerializer.Deserialize<TicketMovieResponse>(ticketContent, options);
        
        //kiểm tra ticket có tồn tại ko
        if (ticket == null)
        {
            return BadRequest(new { message = "Ticket not exist" });
        }
        
        //kiểm tra userId trong request có trùng với userId trong ticket không
        //in thử user id
        Console.WriteLine($"user id trong body: {model.UserId}");
        Console.WriteLine($"user id trong ticket: {ticket.Ticket.UserId}");
        if (ticket.Ticket.UserId != model.UserId)
        {
            return BadRequest(new { message = "UserId is not match" });
        }
        
        //kiểm tra ticket có được xử lý chưa
        if (ticket.Ticket.Status == "processed")
        {
            return BadRequest(new { message = "Ticket has been processed" });
        }
        
        //gán giá vé cho model
        model.Amount = Convert.ToDouble(ticket.Ticket.TotalAmount);
        
        try
        {
            var paymentUrl = _vnPayService.CreatePaymentUrl(model, HttpContext, ticket.Ticket.ExpiryTime.Value);
            return Ok(paymentUrl);   
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new { message = e.Message });
        }
    }
    
    //đường dẫn trả về sau khi thanh toán xong
    [HttpGet]
    [Route("payment_result")]
    public IActionResult PaymentResult()
    {
        Console.WriteLine("PaymentResult API called");  // log xem API có được gọi không
        Console.WriteLine(Request.QueryString); // log trực tiếp Query String
        // log ra dữ liệu nhận được từ VNPAY
        foreach (var (key, value) in Request.Query)
        {
            Console.WriteLine($"{key}: {value}");
        }
        
        var VnPayLibrary = new VnPayLibrary();
        
        //validate vnp_SecureHash và vnp_TmnCode
        var response = VnPayLibrary.GetFullResponseData(Request.Query, _configuration["VnPay:HashSecret"]);
        
        if(!response.Success)
        {
            //nếu thanh toán thất bại
            return BadRequest("Đã có lỗi xảy ra vui lòng thử lại sau");
        }
        
        if (response.VnPayResponseCode == "00")
        {
            // chỉ khi kiểm tra vnp_ResponseCode == "00" thì mới bắt đầu lưu database và gửi message queue
            //1. Lưu dữ liệu vào database
            Domain.Entity.Payment payment = new Domain.Entity.Payment()
            {
                PaymentMethod = response.PaymentMethod,
                TicketId = response.OrderId,
                Status = "processed",
                Amount = response.Amount,
                PaymentId = response.PaymentId,
                CreatedAt = DateTime.Now,
            };
            _paymentService.AddPaymentAsync(payment);
            
            //2. Gửi message queue đến reservation service
            _paymentService.UpdateTicketStatus(response.OrderId, "processed", response.PaymentId, response.PaymentMethod);
            return Ok(response);
        }
        else
        {
            // giao dịch thất bại
            return BadRequest("Giao dịch thất bại");
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        //lấy ra các param phân trang
        var page = int.Parse(Request.Query["page"]);
        var limit = int.Parse(Request.Query["limit"]);
        
        var result = await _paymentService.GetAllAsync(page, limit);
        //chỉnh timezone 
        foreach(var payment in result["records"] as List<Domain.Entity.Payment>)
        {
            payment.CreatedAt = payment.CreatedAt.HasValue
                ? TimeZoneHelper.ConvertToTimeZone(payment.CreatedAt.Value)
                : (DateTime?)null;
        }
        
        return Ok(result);
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        var result = await _paymentService.GetPaymentByIdAsync(id);
        return Ok(result);
    }
    
    //tìm bằng payment id
    [HttpGet]
    [Route("paymentId/{paymentId}")]
    public async Task<IActionResult> GetByPaymentIdAsync(string paymentId)
    {
        var result = await _paymentService.GetByPaymentIdAsync(paymentId);
        return Ok(result);
    }
    
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        var result = await _paymentService.RemovePaymentAsync(id);
        return Ok(result);
    }
    
    //xóa tất cả payment
    [HttpDelete]
    [Route("remove_all")]
    public async Task<IActionResult> RemoveAll()
    {
        var result = await _paymentService.RemoveAll();
        return Ok(result);
    }
    
}