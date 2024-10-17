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
    
    //lấy danh sách vé theo id lịch chiếu
    [HttpGet]
    [Route("schedule/{scheduleId}")]
    public async Task<IActionResult> GetByScheduleIdAsync(string scheduleId)
    {
        var result = await _ticketService.GetByScheduleIdAsync(scheduleId);
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
    
    //lấy tất cả seat trong room và lấy seatdetail theo schedule id
    //cái này dành cho bên movie service khi user muốn đặt mua vé
    [HttpGet]
    [Route("schedule/{scheduleId}/seat/{roomNumber}")]
    public async Task<IActionResult> GetAllBookedSeatByScheduleIdAsync(string scheduleId, string roomNumber)
    {
        var result = await _ticketService.GetAllBookedSeatByScheduleIdAsync(scheduleId, roomNumber);
        return Ok(result);
    }
    
}