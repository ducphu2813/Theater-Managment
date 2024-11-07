using Microsoft.AspNetCore.Mvc;
using ReservationService.Service.Interface;

namespace ReservationService.Controllers.InternalController;

[ApiController]
[Route("internal/[controller]")]
public class TicketInternal : ControllerBase
{
    private readonly ITicketService _ticketService;
    
    public TicketInternal(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }
    
    //lấy tất cả seat trong room và lấy seat detail theo schedule id
    //các service sử dụng: Movie Service
    [HttpGet]
    [Route("schedule/{scheduleId}/seat/{roomNumber}")]
    public async Task<IActionResult> GetAllBookedSeatByScheduleIdAsync(string scheduleId, string roomNumber)
    {
        var result = await _ticketService.GetAllBookedSeatByScheduleIdAsync(scheduleId, roomNumber);
        return Ok(result);
    }
    
    //các service sử dụng: Payment Service
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        var result = await _ticketService.GetByIdAsync(id);
        return Ok(result);
    }
}