using Microsoft.AspNetCore.Mvc;
using ReservationService.Entity.Model;
using ReservationService.Service.Interface;

namespace ReservationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FoodController : ControllerBase
{
    private readonly IFoodService _foodService;
    
    public FoodController(IFoodService foodService)
    {
        _foodService = foodService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _foodService.GetAllAsync();
        return Ok(result);
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        var result = await _foodService.GetByIdAsync(id);
        return Ok(result);
    }
    
    //lấy danh sách food nâng cao
    [HttpGet]
    [Route("getAll")]
    public async Task<IActionResult> GetAllAdvanceAsync()
    {
        //lấy ra các param phân trang
        var page = int.Parse(Request.Query["page"]);
        var limit = int.Parse(Request.Query["limit"]);
        
        //lấy ra các param tìm kiếm nâng cao
        var foodType = Request.Query["foodType"].ToString().Split(',').ToList();
        
        var result = await _foodService.GetAllAdvance(page, limit, foodType);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] Food food)
    {
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _foodService.AddAsync(food);
        return Ok(result);
    }
    
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] Food food)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _foodService.UpdateAsync(id, food);
        return Ok(result);
    }
    
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> RemoveAsync(string id)
    {
        var result = await _foodService.RemoveAsync(id);
        return Ok(result);
    }
    
}