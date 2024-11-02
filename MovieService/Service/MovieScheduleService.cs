
using System.Text.Json;
using MovieService.DTO;
using MovieService.Entity.Model;
using MovieService.Events;
using MovieService.Exceptions;
using MovieService.Helper;
using MovieService.Messaging.Interface;
using MovieService.Repository.Interface;
using MovieService.Service.Interface;

namespace MovieService.Service;

public class MovieScheduleService : IMovieScheduleService
{
    private readonly IMovieScheduleRepository _movieScheduleRepository;
    private readonly IMovieRepository _movieRepository;
    private readonly HttpClient _httpClient;
    // private readonly IPublisher<MovieScheduleEvent> _publisher;
    
    public MovieScheduleService(
        IMovieScheduleRepository movieScheduleRepository,
        IMovieRepository movieRepository,
        // IPublisher<MovieScheduleEvent> publisher,
        IHttpClientFactory httpClientFactory
        )
    {
        _movieScheduleRepository = movieScheduleRepository;
        _movieRepository = movieRepository;
        // _publisher = publisher;
        _httpClient = httpClientFactory.CreateClient("reservation-service");
    }
    
    public async Task<IEnumerable<MovieScheduleDTO>> GetAllAsync()
    {
        //lấy tất cả MovieSchedule
        var movieSchedules = await _movieScheduleRepository.GetAll();
        
        //lấy tất cả movie id từ movie schedule
        var movieIds = movieSchedules.Select(ms => ms.MovieId).Distinct().ToList();
        
        //lấy tất cả movie từ movie id
        var movies = await _movieRepository.GetAllMovieAsyncById(movieIds);
        
        //chuyển đổi thành Dictionary để dễ dàng truy cập
        var movieDict = movies.ToDictionary(m => m.Id);
        
        //chuyển đổi MovieSchedule sang MovieScheduleDTO
        var movieScheduleDtos = movieSchedules.Select(ms => new MovieScheduleDTO
        {
            Id = ms.Id,
            Movie = movieDict.ContainsKey(ms.MovieId) ? movieDict[ms.MovieId] : null,
            RoomNumber = ms.RoomNumber,
            ShowTime = ms.ShowTime,
            SingleSeatPrice = ms.SingleSeatPrice,
            CoupleSeatPrice = ms.CoupleSeatPrice,
            CreatedAt = ms.CreatedAt,
            Status = ms.Status
        }).ToList();
        
        return movieScheduleDtos;

    }
    
    //lấy lịch chiếu theo id phim
    public async Task<List<MovieScheduleDTO>> GetByMovieIdAsync(string movieId)
    {
        //lấy tất cả lịch chiếu theo id phim
        var movieSchedules = await _movieScheduleRepository.GetByMovieIdAsync(movieId);
        
        //lấy movie từ movieId
        var movie = await _movieRepository.GetById(movieId);
        
        //chuyển đổi thành DTO
        var movieScheduleDtos = movieSchedules.Select(ms => new MovieScheduleDTO
        {
            Id = ms.Id,
            Movie = movie,
            RoomNumber = ms.RoomNumber,
            ShowTime = ms.ShowTime,
            SingleSeatPrice = ms.SingleSeatPrice,
            CoupleSeatPrice = ms.CoupleSeatPrice,
            CreatedAt = ms.CreatedAt,
            Status = ms.Status
        }).ToList();
        
        return movieScheduleDtos;
    }

    //lấy lịch chiếu theo id lịch chiếu
    public async Task<Dictionary<String, Object>> GetByIdAsync(string id)
    {
        //note cái cần làm:
        //1. Lấy id schedule gửi qua reservation service để lấy tất cả chỗ ngồi đã đặt
        //2. lấy room number gửi qua reservation service để lấy tất cả chỗ ngồi
        var movieSchedule = await _movieScheduleRepository.GetById(id);
        if (movieSchedule == null)
        {
            throw new NotFoundException($"Movie Schedule with id {id} was not found.");
        }
        
        //chỉnh sửa timezone
        movieSchedule.CreatedAt = movieSchedule.CreatedAt.HasValue
            ? TimeZoneHelper.ConvertToTimeZone(movieSchedule.CreatedAt.Value)
            : (DateTime?)null;
        
        movieSchedule.ShowTime = movieSchedule.ShowTime.HasValue
            ? TimeZoneHelper.ConvertToTimeZone(movieSchedule.ShowTime.Value)
            : (DateTime?)null;
        
        var Movie = await _movieRepository.GetById(movieSchedule.MovieId);
        
        var movieScheduleDto = new MovieScheduleDTO
        {
            Id = movieSchedule.Id,
            Movie = Movie,
            RoomNumber = movieSchedule.RoomNumber,
            ShowTime = movieSchedule.ShowTime,
            SingleSeatPrice = movieSchedule.SingleSeatPrice,
            CoupleSeatPrice = movieSchedule.CoupleSeatPrice,
            CreatedAt = movieSchedule.CreatedAt,
            Status = movieSchedule.Status
        };
        
        //gọi api đến reservation service để lấy tất cả chỗ ngồi và chỗ ngồi đã đặt
        var response = await _httpClient.GetAsync($"/api/Ticket/schedule/{id}/seat/{movieSchedule.RoomNumber}");
        
        //cái này để chuyển từ snake_case sang camelCase
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        var seatInfoContent = await response.Content.ReadAsStringAsync();
        var seatInfo = JsonSerializer.Deserialize<Dictionary<String, Object>>(seatInfoContent, options);
        
        //gửi id lịch chiếu đến queue
        // var movieScheduleEvent = new MovieScheduleEvent
        // {
        //     MovieScheduleId = movieSchedule.Id
        // };
        //
        // _publisher.Publish(movieScheduleEvent);
        
        Dictionary<String, Object> map = new Dictionary<string, object>();
        
        map.Add("movieSchedule", movieScheduleDto);
        map.Add("seatInfo", seatInfo);
        
        return map;
    }
    
    //cũng là hàm lấy theo schedule id nhưng để bên reservation dùng khi bên đó gọi get 1 ticket
    //các service sử dụng: ReservationService
    public async Task<MovieScheduleDTO> GetByScheduleIdAsync(string scheduleId)
    {
        var movieSchedule = await _movieScheduleRepository.GetById(scheduleId);
        
        var Movie = await _movieRepository.GetById(movieSchedule.MovieId);
        
        var movieScheduleDto = new MovieScheduleDTO
        {
            Id = movieSchedule.Id,
            Movie = Movie,
            RoomNumber = movieSchedule.RoomNumber,
            ShowTime = movieSchedule.ShowTime,
            SingleSeatPrice = movieSchedule.SingleSeatPrice,
            CoupleSeatPrice = movieSchedule.CoupleSeatPrice,
            CreatedAt = movieSchedule.CreatedAt,
            Status = movieSchedule.Status
        };
        
        return movieScheduleDto;
    }
    
    //lấy lịch chiếu theo ngày chiếu
    public async Task<List<MovieScheduleDTO>> GetByShowDatesAsync(List<DateTime> showDates)
    {
        //lấy tất cả lịch chiếu theo ngày chiếu
        var movieSchedules = await _movieScheduleRepository.GetByShowDatesAsync(showDates);
        
        //lấy tất cả movie id từ movie schedule
        var movieIds = movieSchedules.Select(ms => ms.MovieId).Distinct().ToList();
        
        //lấy tất cả movie từ movie id
        var movies = await _movieRepository.GetAllMovieAsyncById(movieIds);
        
        //chuyển đổi thành Dictionary để dễ dàng truy cập
        var movieDict = movies.ToDictionary(m => m.Id);
        
        //chuyển đổi MovieSchedule sang MovieScheduleDTO
        var movieScheduleDtos = movieSchedules.Select(ms => new MovieScheduleDTO
        {
            Id = ms.Id,
            Movie = movieDict.ContainsKey(ms.MovieId) ? movieDict[ms.MovieId] : null,
            RoomNumber = ms.RoomNumber,
            ShowTime = ms.ShowTime,
            SingleSeatPrice = ms.SingleSeatPrice,
            CoupleSeatPrice = ms.CoupleSeatPrice,
            CreatedAt = ms.CreatedAt,
            Status = ms.Status
        }).ToList();
        
        return movieScheduleDtos;
    }

    //thêm 1 hoặc nhiều lịch chiếu
    public async Task<List<MovieSchedule>> AddAsync(SaveMovieScheduleDTO movieScheduleDto)
    {
        // gom các lỗi lại để thông báo sau
        var conflictShows = new List<(ScheduleDetail Show1, ScheduleDetail Show2)>();
        
        //lấy movie từ movieId
        var movie = await _movieRepository.GetById(movieScheduleDto.MovieId);
        if (movie == null)
        {
            throw new NotFoundException($"Movie with id {movieScheduleDto.MovieId} was not found.");
        }
        
        //kiểm tra phòng và giờ chiếu có bị đụng giờ không
        //Note
        //1. Lấy tất cả lịch chiếu theo số phòng và những ngày cần tìm
        
        //lấy danh sách số phòng trong lịch chiếu
        var roomNumbers = movieScheduleDto.ScheduleDetails.Select(ms => ms.RoomNumber).ToList();
        //in ra danh sách số phòng lấy được
        Console.WriteLine("Danh sách phòng từ request: ");
        foreach (var roomNumber in roomNumbers)
        {
            Console.WriteLine(roomNumber);
        }
        
        //lấy danh sách ngày từ movieScheduleDto
        var showDates = movieScheduleDto.ScheduleDetails.Select(ms => ms.ShowTime.Value).Distinct().ToList();
        //in ra danh sách ngày lấy được
        Console.WriteLine("Danh sách ngày từ request: ");
        foreach (var showDate in showDates)
        {
            Console.WriteLine(showDate);
        }
        
        //kiểm tra ngày trong movieScheduleDto trước
        foreach (var showDate in showDates)
        {
            if (showDate.Date < DateTime.Now.Date)
            {
                throw new InvalidOperationException("Show time must be greater than to today.");
            }
        }
        
        //kiểm tra các giờ chiếu trong movieScheduleDto nếu chúng cùng phòng với nhau thì phải cách nhau hơn movie.Duration + 30 phút
        //lấy tất cả ScheduleDetail từ movieScheduleDto
        var scheduleDetails = movieScheduleDto.ScheduleDetails;
        //nhóm theo phòng
        var groupedShows = scheduleDetails
            .GroupBy(s => s.RoomNumber)
            .Where(g => g.Count() > 1);
        
        //kiểm tra từng phòng có lịch chiếu trùng
        foreach (var roomShows in groupedShows)
        {
            var sortedShows = roomShows.OrderBy(s => s.ShowTime).ToList();

            for (int i = 0; i < sortedShows.Count - 1; i++)
            {
                var show1 = sortedShows[i];
                var show2 = sortedShows[i + 1];
                var secondDiff = Math.Abs((show2.ShowTime - show1.ShowTime).Value.TotalSeconds);
                
                if(secondDiff < 30*60 + movie.Duration*60)
                {
                    conflictShows.Add((show1, show2));
                }
            }
        }
        
        //nếu có lịch chiếu trùng thì thông báo
        if (conflictShows.Count > 0)
        {
            var message = "Các lịch chiếu bị đụng giờ: ";
            foreach (var (show1, show2) in conflictShows)
            {
                message += $"Phòng {show1.RoomNumber} với lịch: {show1.ShowTime} và {show2.ShowTime}, ";
            }
            throw new InvalidOperationException(message);
        }
        
        //tìm tất cả list lịch chiếu theo số phòng và ngày chiếu
        //Note 1. TÌm theo cả ngày chiếu để giảm số lượng lịch chiếu cần kiểm tra
        var allMovieSchedules = await _movieScheduleRepository.GetByRoomNumbersAndShowDatesAsync(roomNumbers, showDates);
        Console.WriteLine($"Tất cả lịch chiếu tìm được từ database: {allMovieSchedules.Count}");
        //in ra từng lịch chiếu tìm được từ database
        foreach (var movieSchedulesss in allMovieSchedules)
        {
            Console.WriteLine($"Lịch chiếu có được từ database: {movieSchedulesss.RoomNumber} - {movieSchedulesss.ShowTime}");
        }
        
        //sau đó kiểm tra theo số phòng và giờ chiếu, đảm bảo trong vòng 3 tiếng không có 2 lịch chiếu cùng phòng
        //trước đó thì cần đổi timezone trong movieScheduleDto.ScheduleDetails về utc
        foreach (var scheduleDetail in movieScheduleDto.ScheduleDetails)
        {
            scheduleDetail.ShowTime = TimeZoneHelper.ConvertToUtc(scheduleDetail.ShowTime.Value);
        }
        Console.WriteLine("Danh sách ngày từ request sau khi convert: ");
        foreach (var showDate in movieScheduleDto.ScheduleDetails)
        {
            Console.WriteLine(showDate.ShowTime);
        }
        
        if (allMovieSchedules != null && allMovieSchedules.Count > 0)
        {
            Console.WriteLine($"Oke có lịch chiếu ở phòng {roomNumbers} và giờ chiếu {showDates}");
            //lặp qua tất cả lịch chiếu được thêm vào
            foreach (var scheduleDetail in movieScheduleDto.ScheduleDetails)
            {
                //chổ này để test DateTime của C#
                // Console.WriteLine("Kiểm tra lịch chiếu: " + scheduleDetail.ShowTime);
                // Console.WriteLine("Value Lịch chiếu là: " + scheduleDetail.ShowTime.Value);
                // DateTime testShowTime = new DateTime(2024, 11, 13, 03, 00, 00);;
                // testShowTime = testShowTime.AddHours(2);
                // Console.WriteLine("Thử lấy giờ cộng thêm 2 tiếng: " + testShowTime);
                // Console.WriteLine("Trừ 2 cái coi ra cái gì: " + Math.Abs((scheduleDetail.ShowTime - testShowTime).Value.TotalSeconds) );
                
                var isConflict = allMovieSchedules.Any(ms =>
                    ms.RoomNumber == scheduleDetail.RoomNumber &&
                    ms.ShowTime.Value.Date == scheduleDetail.ShowTime.Value.Date && //cái này chỉ để kiểm tra 2 lịch chiếu cùng ngày
                    Math.Abs((ms.ShowTime - scheduleDetail.ShowTime).Value.TotalSeconds) < 30*60 + movie.Duration*60); //cái này để kiểm tra 2 lịch chiếu cùng phòng cách nhau ít nhất 30 phút + thời lượng phim
                
                if (isConflict)
                {
                    throw new InvalidOperationException($"Room {scheduleDetail.RoomNumber} has a schedule conflict with {scheduleDetail.ShowTime}");
                }
            }   
        }
        
        var movieSchedule = movieScheduleDto.ScheduleDetails.Select(ms => new MovieSchedule
        {
            MovieId = movieScheduleDto.MovieId,
            RoomNumber = ms.RoomNumber,
            ShowTime = ms.ShowTime,
            SingleSeatPrice = ms.SingleSeatPrice,
            CoupleSeatPrice = ms.CoupleSeatPrice,
            CreatedAt = ms.CreatedAt,
            Status = ms.Status,
            Duration = movie.Duration
        }).ToList();
        
        var addedMovieSchedule = await _movieScheduleRepository.AddListAsync(movieSchedule);
        return movieSchedule;
    }
    
    // public async Task<List<MovieSchedule>> AddListAsync(List<SaveMovieScheduleDTO> movieScheduleDtos)
    // {
    //     var movieSchedules = movieScheduleDtos.Select(ms => new MovieSchedule
    //     {
    //         MovieId = ms.MovieId,
    //         RoomNumber = ms.RoomNumber,
    //         ShowTime = ms.ShowTime,
    //         SingleSeatPrice = ms.SingleSeatPrice,
    //         CoupleSeatPrice = ms.CoupleSeatPrice,
    //         CreatedAt = ms.CreatedAt,
    //         Status = ms.Status
    //     }).ToList();
    //     
    //     var addedMovieSchedules = await _movieScheduleRepository.AddListAsync(movieSchedules);
    //     return addedMovieSchedules;
    // }

    public async Task<MovieSchedule> UpdateAsync(string id, SaveMovieScheduleDTO movieScheduleDto)
    {
        
        //tìm movie schedule theo id
        var movieScheduleToUpdate = await _movieScheduleRepository.GetById(id);
        
        if (movieScheduleToUpdate == null)
        {
            throw new NotFoundException($"Movie Schedule with id {id} was not found.");
        }
        
        //lấy ra tất cả các property của SaveMovieScheduleDTO
        var dtoProperties = typeof(SaveMovieScheduleDTO).GetProperties();
        //lấy ra tất cả các property của MovieSchedule
        var movieScheduleProperties = typeof(MovieSchedule).GetProperties();
        
        foreach(var dtoProperty in dtoProperties)
        {
            var newValue = dtoProperty.GetValue(movieScheduleDto);

            if (newValue != null)
            {
                var movieScheduleProperty = movieScheduleProperties.FirstOrDefault(
                    mp => mp.Name == dtoProperty.Name);

                if (movieScheduleProperty != null && movieScheduleProperty.CanWrite)
                {
                    if (newValue is string strValue && string.IsNullOrEmpty(strValue)) continue;
                    
                    // đảm bảo rằng giá trị mới khác giá trị hiện tại trước khi cập nhật
                    var currentValue = movieScheduleProperty.GetValue(movieScheduleToUpdate);
                    if (!newValue.Equals(currentValue))
                    {
                        movieScheduleProperty.SetValue(movieScheduleToUpdate, newValue); // Cập nhật giá trị mới vào MovieSchedule
                    }
                }
            }
        }
        
        var updatedMovieSchedule = await _movieScheduleRepository.Update(id, movieScheduleToUpdate);
        return updatedMovieSchedule;
    }

    public async Task<bool> RemoveAsync(string id)
    {
        return await _movieScheduleRepository.Remove(id);
    }
    
    //xóa tất cả lịch chiếu
    public async Task DeleteAll()
    {
        await _movieScheduleRepository.DeleteAll();
    }
}