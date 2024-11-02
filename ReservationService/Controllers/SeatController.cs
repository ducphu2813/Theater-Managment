using Microsoft.AspNetCore.Mvc;
using ReservationService.Entity.Model;
using ReservationService.Service.Interface;

namespace ReservationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeatController : ControllerBase
{
    private readonly ISeatService _seatService;
    
    public SeatController(ISeatService seatService)
    {
        _seatService = seatService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _seatService.GetAllAsync();
        return Ok(result);
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        var result = await _seatService.GetByIdAsync(id);
        return Ok(result);
    }
    
    //lấy theo số phòng
    [HttpGet]
    [Route("room/{roomNumber}")]
    public async Task<IActionResult> GetByRoomNumberAsync(string roomNumber)
    {
        var result = await _seatService.GetByRoomNumberAsync(roomNumber);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] Seat seat)
    {
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _seatService.AddAsync(seat);
        return Ok(result);
    }
    
    //thêm danh sách nhiều ghế
    [HttpPost]
    [Route("addList")]
    public async Task<IActionResult> AddListAsync([FromBody] List<Seat> seats)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _seatService.AddListAsync(seats);
        return Ok(result);
    }
    
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] Seat seat)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _seatService.UpdateAsync(id, seat);
        return Ok(result);
    }
    
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> RemoveAsync(string id)
    {
        var result = await _seatService.RemoveAsync(id);
        return Ok(result);
    }
    
}