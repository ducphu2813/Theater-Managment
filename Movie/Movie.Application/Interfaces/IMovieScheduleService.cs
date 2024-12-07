using Movie.Domain.DTO;
using Movie.Domain.Entity;

namespace Movie.Application.Interfaces;

public interface IMovieScheduleService
{
    Task<Dictionary<string, object>> GetAllAsync(int page, int limit);
    Task<Dictionary<String, Object>> GetByIdAsync(string id);
    Task<List<MovieSchedule>> AddAsync(SaveMovieScheduleDTO movieScheduleDto);
    // Task<List<MovieSchedule>> AddListAsync(List<SaveMovieScheduleDTO> movieScheduleDtos);
    Task<MovieSchedule> UpdateAsync(string id, SaveMovieScheduleDTO movieScheduleDto);
    Task<bool> RemoveAsync(string id);
    
    //hàm lấy lịch chiếu theo phòng
    
    //hàm lấy lịch chiếu theo id phim
    Task<List<MovieScheduleDTO>> GetByMovieIdAsync(string movieId);
    
    //hàm lấy lịch chiếu theo id lịch chiếu cho bên reservation
    Task<MovieScheduleDTO> GetByScheduleIdAsync(string scheduleId);

    //hàm kiểm tra phòng và lịch chiếu có hợp lệ không(truyền vào số phòng và ngày chiếu)
    
    //lấy tất cả lịch chiếu theo danh sách ngày chiếu(tìm theo ngày)
    Task<List<MovieScheduleDTO>> GetByShowDatesAsync(List<DateTime> showDates);
    
    //xóa tất cả movie schedule
    Task DeleteAll();
    
    //lấy ;ịch chiếu năng cao
    Task<Dictionary<string, object>> GetAllAdvance(
        int page
        , int limit
        , List<string>? movieIds
        , List<string>? roomNumbers
        , DateTime? fromShowTimes
        , DateTime? toShowTimes
        , string? sortByShowTime);
}