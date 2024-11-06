using AdminService.Entity.Model;
using AdminService.Helper;
using AdminService.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieSaleController : ControllerBase
{
    
    private readonly IMovieSaleService _movieSaleService;
    
    public MovieSaleController(IMovieSaleService movieSaleService)
    {
        _movieSaleService = movieSaleService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllMovieSale()
    {
        var movieSales = await _movieSaleService.GetAllMovieSaleAsync();
        
        //chỉnh timezone
        foreach (var movieSale in movieSales)
        {
            movieSale.TicketCreatedDate = movieSale.TicketCreatedDate.HasValue
                ? TimeZoneHelper.ConvertToTimeZone(movieSale.TicketCreatedDate.Value)
                : (DateTime?)null;
            movieSale.ShowTime = movieSale.ShowTime.HasValue
                ? TimeZoneHelper.ConvertToTimeZone(movieSale.ShowTime.Value)
                : (DateTime?)null;
        }
        
        return Ok(movieSales);
    }
    
    //xóa tất cả movie sale
    [HttpDelete]
    [Route("deleteAll")]
    public async Task<IActionResult> DeleteAll()
    {
        await _movieSaleService.DeleteAll();
        
        return Ok();
    }
    
    //tìm theo id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovieSaleById(string id)
    {
        var movieSale = await _movieSaleService.GetMovieSaleByIdAsync(id);
        
        //chỉnh timezone
        movieSale.TicketCreatedDate = movieSale.TicketCreatedDate.HasValue
            ? TimeZoneHelper.ConvertToTimeZone(movieSale.TicketCreatedDate.Value)
            : (DateTime?)null;
        movieSale.ShowTime = movieSale.ShowTime.HasValue
            ? TimeZoneHelper.ConvertToTimeZone(movieSale.ShowTime.Value)
            : (DateTime?)null;
        
        return Ok(movieSale);
    }
    
    //tìm theo payment id
    [HttpGet("paymentId/{paymentId}")]
    public async Task<IActionResult> GetByPaymentId(string paymentId)
    {
        var movieSale = await _movieSaleService.GetByPaymentId(paymentId);
        
        //chỉnh timezone
        movieSale.TicketCreatedDate = movieSale.TicketCreatedDate.HasValue
            ? TimeZoneHelper.ConvertToTimeZone(movieSale.TicketCreatedDate.Value)
            : (DateTime?)null;
        movieSale.ShowTime = movieSale.ShowTime.HasValue
            ? TimeZoneHelper.ConvertToTimeZone(movieSale.ShowTime.Value)
            : (DateTime?)null;
        
        return Ok(movieSale);
    }
    
    //tìm theo user id
    [HttpGet("userId/{userId}")]
    public async Task<IActionResult> GetByUserId(string userId)
    {
        var movieSales = await _movieSaleService.GetByUserId(userId);
        
        //chỉnh timezone
        foreach (var movieSale in movieSales)
        {
            movieSale.TicketCreatedDate = movieSale.TicketCreatedDate.HasValue
                ? TimeZoneHelper.ConvertToTimeZone(movieSale.TicketCreatedDate.Value)
                : (DateTime?)null;
            movieSale.ShowTime = movieSale.ShowTime.HasValue
                ? TimeZoneHelper.ConvertToTimeZone(movieSale.ShowTime.Value)
                : (DateTime?)null;
        }
        
        return Ok(movieSales);
    }
    
    //tìm theo 1 khoảng cách ngày, tìm theo TicketCreatedDate, ví dụ GET /api/MovieSale/dateRange?from=2024-06-15&to=2024-06-16
    [HttpGet("dateRange")]
    public async Task<IActionResult> FindByDateRangeAsync()
    {
        var fromQuery = Request.Query["from"].ToString();
        var toQuery = Request.Query["to"].ToString();
        
        if (!DateTime.TryParse(fromQuery, out DateTime from) || !DateTime.TryParse(toQuery, out DateTime to))
        {
            return BadRequest("Invalid date format. Please use YYYY-MM-DD format.");
        }
        
        //chuyển về DateTime
        from = new DateTime(from.Year, from.Month, from.Day, 0, 0, 0);
        to = new DateTime(to.Year, to.Month, to.Day, 23, 59, 59);
        
        var movieSales = await _movieSaleService.FindByDateRangeAsync(from, to);
        
        //chỉnh timezone
        foreach (var movieSale in movieSales)
        {
            movieSale.TicketCreatedDate = movieSale.TicketCreatedDate.HasValue
                ? TimeZoneHelper.ConvertToTimeZone(movieSale.TicketCreatedDate.Value)
                : (DateTime?)null;
            movieSale.ShowTime = movieSale.ShowTime.HasValue
                ? TimeZoneHelper.ConvertToTimeZone(movieSale.ShowTime.Value)
                : (DateTime?)null;
        }
        
        return Ok(movieSales);
    }
    
}