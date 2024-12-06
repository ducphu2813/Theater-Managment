using Microsoft.AspNetCore.Mvc;
using MovieService.Core.Entity.Model;
using MovieService.Core.Interfaces.Service;

namespace MovieService.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class RoomController : ControllerBase
{
    
    private readonly IRoomService _roomService;
    
    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        //lấy ra các param phân trang
        var page = int.Parse(Request.Query["page"]);
        var limit = int.Parse(Request.Query["limit"]);
        //in thử các param phân trang
        // Console.WriteLine($"Page: {page}");
        // Console.WriteLine($"Limit: {limit}");
        var result = await _roomService.GetAllAsync(page, limit);
        return Ok(result);
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        var result = await _roomService.GetByIdAsync(id);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] Room room)
    {
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _roomService.AddAsync(room);
        return Ok(result);
    }
    
    [HttpPost]
    [Route("list")]
    public async Task<IActionResult> AddListAsync([FromBody] List<Room> rooms)
    {
        var result = await _roomService.AddListAsync(rooms);
        return Ok(result);
    }
    
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] Room room)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _roomService.UpdateAsync(id, room);
        return Ok(result);
    }
    
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> RemoveAsync(string id)
    {
        var result = await _roomService.RemoveAsync(id);
        return Ok(result);
    }
    
}