using Microsoft.AspNetCore.Mvc;
using ReservationService.Core.Entity.Model;
using ReservationService.Core.Interfaces.Service;

namespace ReservationService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiscountController : ControllerBase
{
    private readonly IDiscountService _discountService;
    
    public DiscountController(IDiscountService discountService)
    {
        _discountService = discountService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _discountService.GetAllAsync();
        return Ok(result);
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        var result = await _discountService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet]
    [Route("getAll")]
    public async Task<IActionResult> GetAllAdvanceAsync()
    {
        //lấy ra các param phân trang
        var page = int.Parse(Request.Query["page"]);
        var limit = int.Parse(Request.Query["limit"]);
        
        var result = await _discountService.GetAllAdvance(page, limit);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] Discount discount)
    {
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _discountService.AddAsync(discount);
        return Ok(result);
    }
    
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] Discount discount)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _discountService.UpdateAsync(id, discount);
        return Ok(result);
    }
    
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> RemoveAsync(string id)
    {
        var result = await _discountService.RemoveAsync(id);
        return Ok(result);
    }
    
}