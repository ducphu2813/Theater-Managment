
using System.Text.Json;
using MovieService.DTO;
using MovieService.Entity.Model;
using MovieService.Events;
using MovieService.Exceptions;
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

    public async Task<List<MovieSchedule>> AddAsync(SaveMovieScheduleDTO movieScheduleDto)
    {
        //Note cái sẽ làm:
        //1. Kiểm tra xem phim có tồn tại không
        //2. Kiểm tra phòng và giờ chiếu có hợp lệ không
        var movieSchedule = movieScheduleDto.ScheduleDetails.Select(ms => new MovieSchedule
        {
            MovieId = movieScheduleDto.MovieId,
            RoomNumber = ms.RoomNumber,
            ShowTime = ms.ShowTime,
            SingleSeatPrice = ms.SingleSeatPrice,
            CoupleSeatPrice = ms.CoupleSeatPrice,
            CreatedAt = ms.CreatedAt,
            Status = ms.Status
        }).ToList();
        
        var addedMovieSchedule = await _movieScheduleRepository.AddListAsync(movieSchedule);
        return addedMovieSchedule;
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
                    
                    // Đảm bảo rằng giá trị mới khác giá trị hiện tại trước khi cập nhật
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
}