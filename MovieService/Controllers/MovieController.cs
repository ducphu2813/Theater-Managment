using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MovieService.DTO;
using MovieService.Entity.Model;
using MovieService.Service.Interface;

namespace MovieService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;
    
    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _movieService.GetAllAsync();
        return Ok(result);
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        var result = await _movieService.GetByIdAsync(id);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] MovieDTO movieDto)
    {
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _movieService.AddAsync(movieDto);
        return Ok(result);
    }
    
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] MovieDTO movieDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _movieService.UpdateAsync(id, movieDto);
        return Ok(result);
    }
    
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> RemoveAsync(string id)
    {
        var result = await _movieService.RemoveAsync(id);
        return Ok(result);
    }
    
    
}