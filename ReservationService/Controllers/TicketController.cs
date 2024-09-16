using Microsoft.AspNetCore.Mvc;
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
    
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        var result = await _ticketService.GetByIdAsync(id);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] Ticket ticket)
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
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] Ticket ticket)
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
}