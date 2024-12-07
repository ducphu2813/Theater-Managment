using Microsoft.AspNetCore.Mvc;
using Reservation.Application.Interfaces.Service;
using Reservation.Domain.Entity;

namespace Reservation.API.Controllers;


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
        //lấy ra các param phân trang
        var page = int.Parse(Request.Query["page"]);
        var limit = int.Parse(Request.Query["limit"]);
        
        var result = await _seatService.GetAllAsync(page, limit);
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
    
    //update nhiều seat
    [HttpPut]
    [Route("updateList")]
    public async Task<IActionResult> UpdateListAsync([FromBody] List<Seat> seats)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _seatService.UpdateListAsync(seats);
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