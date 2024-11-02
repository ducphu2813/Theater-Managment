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
    
    //lấy tất cả lịch chiếu theo danh sách ngày chiếu(tìm theo ngày)
    public async Task<List<MovieSchedule>> GetByShowDatesAsync(List<DateTime> showDates)
    {
        var filters = new List<FilterDefinition<MovieSchedule>>();

        foreach (var date in showDates)
        {
            var startOfDay = date.Date; // Thời điểm bắt đầu ngày
            var endOfDay = date.Date.AddDays(1).AddTicks(-1); // Thời điểm cuối ngày

            // Tạo bộ lọc cho từng ngày, kiểm tra nếu ShowTime nằm trong khoảng ngày đó
            var filter = Builders<MovieSchedule>.Filter.And(
                Builders<MovieSchedule>.Filter.Gte(x => x.ShowTime, startOfDay),
                Builders<MovieSchedule>.Filter.Lte(x => x.ShowTime, endOfDay)
            );

            filters.Add(filter);
        }

        var combinedFilter = Builders<MovieSchedule>.Filter.Or(filters);
        var result = await _collection.Find(combinedFilter).ToListAsync();
    
        return result;
    }
    
    //lấy theo ngày chiếu và số phòng
    public async Task<List<MovieSchedule>> GetByRoomNumbersAndShowDatesAsync(List<string> roomNumbers, List<DateTime> showDates)
    {
        var filters = new List<FilterDefinition<MovieSchedule>>();
    
        // Bộ lọc cho danh sách số phòng
        if (roomNumbers != null && roomNumbers.Count > 0)
        {
            filters.Add(Builders<MovieSchedule>.Filter.In(x => x.RoomNumber, roomNumbers));
        }
    
        // Bộ lọc cho danh sách ngày chiếu
        if (showDates != null && showDates.Count > 0)
        {
            var dateFilters = new List<FilterDefinition<MovieSchedule>>();
    
            foreach (var date in showDates)
            {
                var startOfDay = date.Date;
                var endOfDay = date.Date.AddDays(1).AddTicks(-1);
    
                var dateFilter = Builders<MovieSchedule>.Filter.And(
                    Builders<MovieSchedule>.Filter.Gte(x => x.ShowTime, startOfDay),
                    Builders<MovieSchedule>.Filter.Lte(x => x.ShowTime, endOfDay)
                );
    
                dateFilters.Add(dateFilter);
            }
    
            // Kết hợp tất cả bộ lọc ngày bằng toán tử Or
            filters.Add(Builders<MovieSchedule>.Filter.Or(dateFilters));
        }
    
        // Kết hợp các bộ lọc cho phòng và ngày bằng And
        var combinedFilter = Builders<MovieSchedule>.Filter.And(filters);
        
        // Thực hiện truy vấn với bộ lọc kết hợp
        return await _collection.Find(combinedFilter).ToListAsync();
    }
    
    //hàm xóa tất cả movie schedule
    public async Task DeleteAll()
    {
        await _collection.DeleteManyAsync(Builders<MovieSchedule>.Filter.Empty);
    }

    
}