using MongoDB.Driver;
using MovieService.Context;
using MovieService.Entity.Model;
using MovieService.Repository.Interface;
using MovieService.Repository.MongoDBRepo;

namespace MovieService.Repository;

public class MovieScheduleRepository : MongoDBRepository<MovieSchedule>, IMovieScheduleRepository
{
    public MovieScheduleRepository(MongoDBContext context) : base(context, "MovieSchedule")
    {
        
    }
    
    //hàm thêm 1 danh sách movie schedule
    public async Task<List<MovieSchedule>> AddListAsync(List<MovieSchedule> movieSchedules)
    {
        await _collection.InsertManyAsync(movieSchedules);
        return movieSchedules;
    }
    
    // lấy lịch chiếu theo id phim
    public async Task<List<MovieSchedule>> GetByMovieIdAsync(string movieId)
    {
        var filter = Builders<MovieSchedule>.Filter.Eq("MovieId", movieId); //Filter.Eq để tìm theo 1 giá trị
        return await _collection.Find(filter).ToListAsync();
    }
    
    //lấy các lịch chiếu theo danh sách số phòng
    public async Task<List<MovieSchedule>> GetByRoomNumbersAsync(List<string> roomNumbers)
    {
        var filter = Builders<MovieSchedule>.Filter.In("RoomNumber", roomNumbers); //Filter.In để tìm theo 1 danh sách giá trị
        return await _collection.Find(filter).ToListAsync();
    }
    
}