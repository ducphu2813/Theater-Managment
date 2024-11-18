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

    //tìm nâng cao
    public async Task<Dictionary<string, object>> GetAllAdvance(
        int page
        , int limit
        , List<string>? movieIds
        , List<string>? roomNumbers
        , DateTime? fromShowTimes
        , DateTime? toShowTimes
        , string? sortByShowTime)
    {
        // tạo một list để lưu các sub filter
        var subFilters = new List<FilterDefinition<MovieSchedule>>();

        // param movieIds
        if (movieIds != null && movieIds.Count > 0)
        {
            var movieIdFilters = movieIds.Select(m => Builders<MovieSchedule>.Filter.Eq(x => x.MovieId, m));
            subFilters.Add(Builders<MovieSchedule>.Filter.Or(movieIdFilters));
        }

        // param roomNumbers
        if (roomNumbers != null && roomNumbers.Count > 0)
        {
            var roomNumberFilters = roomNumbers.Select(r => Builders<MovieSchedule>.Filter.Eq(x => x.RoomNumber, r));
            subFilters.Add(Builders<MovieSchedule>.Filter.Or(roomNumberFilters));
        }

        // param fromShowTimes
        if (fromShowTimes != DateTime.MinValue)
        {
            var fromShowTimeFilter = Builders<MovieSchedule>.Filter.Gte(x => x.ShowTime, fromShowTimes);
            subFilters.Add(fromShowTimeFilter);
        }

        // param toShowTimes
        if (toShowTimes != DateTime.MinValue)
        {
            var toShowTimeFilter = Builders<MovieSchedule>.Filter.Lte(x => x.ShowTime, toShowTimes);
            subFilters.Add(toShowTimeFilter);
        }
    
        //gộp filter lại bằng OR
        var combinedFilter = subFilters.Count > 0 ? Builders<MovieSchedule>.Filter.Or(subFilters) : Builders<MovieSchedule>.Filter.Empty;
        
        //tạo sort theo ShowTime
        SortDefinition<MovieSchedule> sortDefinition = Builders<MovieSchedule>.Sort.Descending(x => x.ShowTime);
        
        if(sortByShowTime == "asc")
        {
            sortDefinition = Builders<MovieSchedule>.Sort.Ascending(x => x.ShowTime);
        }
        
        //lấy total records
        var total = await _collection.CountDocumentsAsync(combinedFilter);
        
        //lấy data
        var data = await _collection
            .Find(combinedFilter)           //sử dụng filter
            .Sort(sortDefinition)           //sắp xếp
            .Skip((page - 1) * limit)       //bỏ qua
            .Limit(limit)                   //giới hạn
            .ToListAsync();

        // Tạo dữ liệu phân trang
        var paging = new Dictionary<string, object>
        {
            {"totalRecords", total},
            {"records", data},
            {"page", page},
            {"limit", limit}
        };

        return paging;
    }
    
}