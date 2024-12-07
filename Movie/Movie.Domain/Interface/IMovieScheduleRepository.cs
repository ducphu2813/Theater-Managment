﻿using Movie.Domain.Entity;
using Shared.Interfaces;

namespace Movie.Domain.Interface;

public interface IMovieScheduleRepository : IRepository<MovieSchedule>
{
    Task<List<MovieSchedule>> AddListAsync(List<MovieSchedule> movieSchedules);
    
    // lấy lịch chiếu theo id phim
    Task<List<MovieSchedule>> GetByMovieIdAsync(string movieId);
    
    //lấy các lịch chiếu theo danh sách số phòng
    Task<List<MovieSchedule>> GetByRoomNumbersAsync(List<string> roomNumbers);
    
    //lấy tất cả lịch chiếu theo danh sách ngày chiếu(tìm theo ngày)
    Task<List<MovieSchedule>> GetByShowDatesAsync(List<DateTime> showDates);
    
    //lấy theo ngày chiếu và số phòng
    Task<List<MovieSchedule>> GetByRoomNumbersAndShowDatesAsync(List<string> roomNumbers, List<DateTime> showDates);
    
    //hàm xóa tất cả movie schedule
    Task DeleteAll();
    
    //tìm nâng cao
    Task<Dictionary<string, object>> GetAllAdvance(
        int page
        , int limit
        , List<string>? movieIds
        , List<string>? roomNumbers
        , DateTime? fromShowTimes
        , DateTime? toShowTimes
        , string? sortByShowTime);
}