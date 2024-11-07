using Microsoft.AspNetCore.Authorization;
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
        //lấy ra các param phân trang
        var page = int.Parse(Request.Query["page"]);
        var limit = int.Parse(Request.Query["limit"]);
        //in thử các param phân trang
        Console.WriteLine($"Page: {page}");
        Console.WriteLine($"Limit: {limit}");
        
        var result = await _movieService.GetAllAsync(page, limit);
        
        //trả về tổng hợp dictionary gồm result + page + limit
        return Ok(result);
    }
    
    //lấy tất cả movie nâng cao
    [HttpGet]
    [Route("getAll")]
    public async Task<IActionResult> GetAllAdvanceAsync()
    {
        //lấy ra các param phân trang
        var page = int.Parse(Request.Query["page"]);
        var limit = int.Parse(Request.Query["limit"]);
        
        //lấy ra các param tìm kiếm nâng cao
        var name = Request.Query["name"]
            .ToString()     //convert thành string     
            .Split(",", StringSplitOptions.RemoveEmptyEntries)  //tách theo dấu ,
            .ToList();  //thành mảng
        var director = Request.Query["director"]
            .ToString()          
            .Split(",", StringSplitOptions.RemoveEmptyEntries)  
            .ToList();  
        var actor = Request.Query["actor"]
            .ToString()          
            .Split(",", StringSplitOptions.RemoveEmptyEntries)  
            .ToList();  
        var author = Request.Query["author"]
            .ToString()          
            .Split(",", StringSplitOptions.RemoveEmptyEntries)  
            .ToList();  
        var dub = Request.Query["dub"]
            .ToString()          
            .Split(",", StringSplitOptions.RemoveEmptyEntries)  
            .ToList();  
        var subtitle = Request.Query["subtitle"]
            .ToString()
            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            .ToList();
        var genres = Request.Query["genres"]
            .ToString()
            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            .ToList();
        
        var result = await _movieService.GetAllAdvance(page, limit, name, director, actor, author, dub, subtitle, genres);
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