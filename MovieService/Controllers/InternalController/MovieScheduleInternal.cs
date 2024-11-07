using Microsoft.AspNetCore.Mvc;
using MovieService.Service.Interface;

namespace MovieService.Controllers.InternalController;

[ApiController]
[Route("internal/[controller]")]
public class MovieScheduleInternal : ControllerBase
{
    private readonly IMovieScheduleService _movieScheduleService;
    
    public MovieScheduleInternal(IMovieScheduleService movieScheduleService)
    {
        _movieScheduleService = movieScheduleService;
    }
    
    //hàm lấy movie schedule theo schedule id cho bên reservation dùng khi bên đó gọi get 1 ticket
    //các service sử dụng: Reservation Service
    [HttpGet]
    [Route("schedule/{scheduleId}")]
    public async Task<IActionResult> GetByScheduleIdAsync(string scheduleId)
    {
        var result = await _movieScheduleService.GetByScheduleIdAsync(scheduleId);
        return Ok(result);
    }
}