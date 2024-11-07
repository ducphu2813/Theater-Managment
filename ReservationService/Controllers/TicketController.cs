using Microsoft.AspNetCore.Mvc;
using ReservationService.DTO;
using ReservationService.Entity.Model;
using ReservationService.Service.Interface;

namespace ReservationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketController : ControllerBase
{
    
    private readonly ITicketService _ticketService;
    
    public TicketController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _ticketService.GetAllAsync();
        return Ok(result);
    }
    
    //lấy tất cả ticket nâng cao
    [HttpGet]
    [Route("getAll")]
    public async Task<IActionResult> GetAllAdvanceAsync()
    {
        //lấy ra các param phân trang
        var page = int.Parse(Request.Query["page"]);
        var limit = int.Parse(Request.Query["limit"]);
        
        //lấy ra các param tìm kiếm nâng cao
        var userId = Request.Query["userId"];
        //lấy scheduleId từ param, lưu thành 1 mảng string
        var scheduleId = Request.Query["scheduleId"]
            .ToString()          //convert thành string
            .Split(",", StringSplitOptions.RemoveEmptyEntries)  //tách theo dấu ,
            .ToList();  //thành mảng
        var status = Request.Query["status"];
        // lấy fromCreateDate và toCreateDate từ param
        var fromCreateDate = DateTime.TryParse(Request.Query["fromCreateDate"], out var parsedFromCreateDate)
            ? parsedFromCreateDate.ToUniversalTime()
            : DateTime.MinValue; // giá trị mặc định
        var toCreateDate = DateTime.TryParse(Request.Query["toCreateDate"], out var parsedToCreateDate)
            ? parsedToCreateDate.ToUniversalTime()
            : DateTime.MinValue; // giá trị mặc định

        // lấy fromTotalPrice và toTotalPrice từ param
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
        
        var result = await _ticketService.GetAllAdvance(
            page
            , limit
            , userId
            , scheduleId
            , status
            , fromCreateDate
            , toCreateDate
            , fromTotalPrice
            , toTotalPrice
            , sortByCreateDate
            , sortByTotalPrice);
        
        return Ok(result);
    }
    
    //lấy danh sách vé theo id lịch chiếu
    [HttpGet]
    [Route("schedule/{scheduleId}")]
    public async Task<IActionResult> GetByScheduleIdAsync(string scheduleId)
    {
        var result = await _ticketService.GetByScheduleIdAsync(scheduleId);
        return Ok(result);
    }
    
    //lấy theo user id
    [HttpGet]
    [Route("user/{userId}")]
    public async Task<IActionResult> GetByUserIdAsync(string userId)
    {
        var result = await _ticketService.GetByUserIdAsync(userId);
        return Ok(result);
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        var result = await _ticketService.GetByIdAsync(id);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] SaveTicketDTO ticket)
    {
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _ticketService.AddAsync(ticket);
        return Ok(result);
    }
    
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] UpdateTicketDTO ticket)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _ticketService.UpdateAsync(id, ticket);
        return Ok(result);
    }
    
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> RemoveAsync(string id)
    {
        var result = await _ticketService.RemoveAsync(id);
        return Ok(result);
    }
    
    //hàm xóa tất cả vé
    [HttpDelete]
    [Route("removeAll")]
    public async Task<IActionResult> RemoveAllAsync()
    {
        var result = await _ticketService.RemoveAllAsync();
        return Ok(result);
    }
    
}