
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
    private readonly IPublisher<MovieScheduleEvent> _publisher;
    
    public MovieScheduleService(
        IMovieScheduleRepository movieScheduleRepository,
        IMovieRepository movieRepository,
        IPublisher<MovieScheduleEvent> publisher)
    {
        _movieScheduleRepository = movieScheduleRepository;
        _movieRepository = movieRepository;
        _publisher = publisher;
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

    //lấy lịch chiếu theo id
    public async Task<MovieScheduleDTO> GetByIdAsync(string id)
    {
        var movieSchedule = await _movieScheduleRepository.GetById(id);
        if (movieSchedule == null)
        {
            throw new NotFoundException($"Movie with id {id} was not found.");
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
        
        //gửi id lịch chiếu đến queue
        var movieScheduleEvent = new MovieScheduleEvent
        {
            MovieScheduleId = movieSchedule.Id
        };
        
        _publisher.Publish(movieScheduleEvent);
        
        return movieScheduleDto;
    }

    public async Task<MovieSchedule> AddAsync(SaveMovieScheduleDTO movieScheduleDto)
    {
        var movieSchedule = new MovieSchedule
        {
            MovieId = movieScheduleDto.MovieId,
            RoomNumber = movieScheduleDto.RoomNumber,
            ShowTime = movieScheduleDto.ShowTime,
            SingleSeatPrice = movieScheduleDto.SingleSeatPrice,
            CoupleSeatPrice = movieScheduleDto.CoupleSeatPrice,
            CreatedAt = movieScheduleDto.CreatedAt,
            Status = movieScheduleDto.Status
        };
        
        var addedMovieSchedule = await _movieScheduleRepository.Add(movieSchedule);
        return addedMovieSchedule;
    }
    
    public async Task<List<MovieSchedule>> AddListAsync(List<SaveMovieScheduleDTO> movieScheduleDtos)
    {
        var movieSchedules = movieScheduleDtos.Select(ms => new MovieSchedule
        {
            MovieId = ms.MovieId,
            RoomNumber = ms.RoomNumber,
            ShowTime = ms.ShowTime,
            SingleSeatPrice = ms.SingleSeatPrice,
            CoupleSeatPrice = ms.CoupleSeatPrice,
            CreatedAt = ms.CreatedAt,
            Status = ms.Status
        }).ToList();
        
        var addedMovieSchedules = await _movieScheduleRepository.AddListAsync(movieSchedules);
        return addedMovieSchedules;
    }

    public async Task<MovieSchedule> UpdateAsync(string id, SaveMovieScheduleDTO movieScheduleDto)
    {
        var movieSchedule = new MovieSchedule
        {
            Id = id,
            MovieId = movieScheduleDto.MovieId,
            RoomNumber = movieScheduleDto.RoomNumber,
            ShowTime = movieScheduleDto.ShowTime,
            SingleSeatPrice = movieScheduleDto.SingleSeatPrice,
            CoupleSeatPrice = movieScheduleDto.CoupleSeatPrice,
            CreatedAt = movieScheduleDto.CreatedAt,
            Status = movieScheduleDto.Status
        };
        
        var updatedMovieSchedule = await _movieScheduleRepository.Update(id, movieSchedule);
        return updatedMovieSchedule;
    }

    public async Task<bool> RemoveAsync(string id)
    {
        return await _movieScheduleRepository.Remove(id);
    }
}