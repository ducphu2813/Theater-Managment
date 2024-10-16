﻿using Microsoft.AspNetCore.Mvc;
using MovieService.DTO;
using MovieService.Service.Interface;

namespace MovieService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieScheduleController : ControllerBase
{
    private readonly IMovieScheduleService _movieScheduleService;
    
    public MovieScheduleController(IMovieScheduleService movieScheduleService)
    {
        _movieScheduleService = movieScheduleService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _movieScheduleService.GetAllAsync();
        return Ok(result);
    }
    
    //lấy lịch chiếu theo id phim
    [HttpGet]
    [Route("movie/{movieId}")]
    public async Task<IActionResult> GetByMovieIdAsync(string movieId)
    {
        var result = await _movieScheduleService.GetByMovieIdAsync(movieId);
        return Ok(result);
    }
    
    //get by id
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        var result = await _movieScheduleService.GetByIdAsync(id);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] SaveMovieScheduleDTO movieScheduleDto)
    {
        var result = await _movieScheduleService.AddAsync(movieScheduleDto);
        return Ok(result);
    }
    
    // [HttpPost]
    // [Route("list")]
    // public async Task<IActionResult> AddListAsync([FromBody] List<SaveMovieScheduleDTO> movieScheduleDtos)
    // {
    //     var result = await _movieScheduleService.AddListAsync(movieScheduleDtos);
    //     return Ok(result);
    // }
    
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] SaveMovieScheduleDTO movieScheduleDto)
    {
        var result = await _movieScheduleService.UpdateAsync(id, movieScheduleDto);
        return Ok(result);
    }
    
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> RemoveAsync(string id)
    {
        var result = await _movieScheduleService.RemoveAsync(id);
        return Ok(result);
    }
    
    //hàm lấy movie schedule theo schedule id cho bên reservation dùng khi bên đó gọi get 1 ticket
    [HttpGet]
    [Route("schedule/{scheduleId}")]
    public async Task<IActionResult> GetByScheduleIdAsync(string scheduleId)
    {
        var result = await _movieScheduleService.GetByScheduleIdAsync(scheduleId);
        return Ok(result);
    }
    
}