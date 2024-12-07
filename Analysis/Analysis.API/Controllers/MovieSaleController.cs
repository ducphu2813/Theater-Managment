using Analysis.Application.Helper;
using Analysis.Application.Interfaces;
using Analysis.Domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Analysis.API.Controllers;


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
        //lấy ra các param phân trang
        var page = int.Parse(Request.Query["page"]);
        var limit = int.Parse(Request.Query["limit"]);
        
        var movieSales = await _movieSaleService.GetAllAsync(page, limit);
        
        //chỉnh timezone
        foreach (var movieSale in movieSales["records"] as List<MovieSale>)
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
    
    //tìm nâng cao
    [HttpGet]
    [Route("getAll")]
    public async Task<IActionResult> GetAllAdvance()
    {
        //lấy ra các param phân trang
        var page = int.Parse(Request.Query["page"]);
        var limit = int.Parse(Request.Query["limit"]);
        
        //lấy ra các param tìm kiếm nâng cao
        var movieId = Request.Query["movieId"]
            .ToString()          //convert thành string
            .Split(",", StringSplitOptions.RemoveEmptyEntries)  //tách theo dấu ,
            .ToList();  //thành mảng
        var genres = Request.Query["genres"]
            .ToString()          //convert thành string
            .Split(",", StringSplitOptions.RemoveEmptyEntries)  //tách theo dấu ,
            .ToList();  //thành mảng
        var fromCreateDate = DateTime.TryParse(Request.Query["fromCreateDate"], out var parsedFromCreateDate)
            ? parsedFromCreateDate.ToUniversalTime()
            : DateTime.MinValue; // giá trị mặc định
        var toCreateDate = DateTime.TryParse(Request.Query["toCreateDate"], out var parsedToCreateDate)
            ? parsedToCreateDate.ToUniversalTime()
            : DateTime.MinValue; // giá trị mặc định
        var fromTotalPrice = float.TryParse(Request.Query["fromTotalPrice"], out var parsedFromTotalPrice)
            ? parsedFromTotalPrice
            : 0; // giá trị mặc định
        var toTotalPrice = float.TryParse(Request.Query["toTotalPrice"], out var parsedToTotalPrice)
            ? parsedToTotalPrice
            : 0; // giá trị mặc định
        //lấy sort từ param
        string sortByCreateDate = "";
        string sortByTotalPrice = "";
        sortByCreateDate = Request.Query["sortByCreateDate"].ToString().ToLower(); 
        sortByTotalPrice = Request.Query["sortByTotalPrice"].ToString().ToLower();
        
        var result = await _movieSaleService.GetAllAdvance(
            page
            , limit
            , movieId
            , genres
            , fromCreateDate
            , toCreateDate
            , fromTotalPrice
            , toTotalPrice
            , sortByCreateDate
            , sortByTotalPrice);
        
        return Ok(result);
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