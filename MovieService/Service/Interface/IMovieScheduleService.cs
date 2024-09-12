using MovieService.DTO;
using MovieService.Entity.Model;

namespace MovieService.Service.Interface;

public interface IMovieScheduleService
{
    Task<IEnumerable<MovieScheduleDTO>> GetAllAsync();
    Task<MovieScheduleDTO> GetByIdAsync(string id);
    Task<MovieSchedule> AddAsync(SaveMovieScheduleDTO movieScheduleDto);
    Task<MovieSchedule> UpdateAsync(string id, SaveMovieScheduleDTO movieScheduleDto);
    Task<bool> RemoveAsync(string id);
}