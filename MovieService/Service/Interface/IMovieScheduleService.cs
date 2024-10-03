using MovieService.DTO;
using MovieService.Entity.Model;

namespace MovieService.Service.Interface;

public interface IMovieScheduleService
{
    Task<IEnumerable<MovieScheduleDTO>> GetAllAsync();
    Task<MovieScheduleDTO> GetByIdAsync(string id);
    Task<MovieSchedule> AddAsync(SaveMovieScheduleDTO movieScheduleDto);
    Task<List<MovieSchedule>> AddListAsync(List<SaveMovieScheduleDTO> movieScheduleDtos);
    Task<MovieSchedule> UpdateAsync(string id, SaveMovieScheduleDTO movieScheduleDto);
    Task<bool> RemoveAsync(string id);
    
    //hàm lấy lịch chiếu theo phòng
    
    //hàm lấy lịch chiếu theo id phim
    Task<List<MovieScheduleDTO>> GetByMovieIdAsync(string movieId);
    
    //hàm kiểm tra phòng và lịch chiếu có hợp lệ không(truyền vào số phòng và ngày chiếu)
}