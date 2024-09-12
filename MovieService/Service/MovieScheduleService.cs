using MovieService.DTO;
using MovieService.Entity.Model;
using MovieService.Exceptions;
using MovieService.Repository.Interface;
using MovieService.Service.Interface;

namespace MovieService.Service;

public class MovieScheduleService : IMovieScheduleService
{
    private readonly IMovieScheduleRepository _movieScheduleRepository;
    private readonly IMovieRepository _movieRepository;
    
    public MovieScheduleService(
        IMovieScheduleRepository movieScheduleRepository,
        IMovieRepository movieRepository)
    {
        _movieScheduleRepository = movieScheduleRepository;
        _movieRepository = movieRepository;
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