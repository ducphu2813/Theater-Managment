using MongoDB.Driver;
using Movie.Application.Interfaces;
using Movie.Domain.DTO;
using Movie.Domain.Entity;
using Movie.Domain.Interface;

namespace Movie.Application.Service;

public class MovieService : IMovieService
{
    
    private readonly IMovieRepository _movieRepository;
    
    public MovieService(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }
    
    public async Task<Dictionary<string, object>> GetAllAsync(int page, int limit)
    {
        return await _movieRepository.GetAll(page, limit);
    }
    
    //hàm tìm movie nâng cao
    public async Task<Dictionary<string, object>> GetAllAdvance(
        int page
        , int limit
        , List<string> name
        , List<string> director
        , List<string> actor
        , List<string> author
        , List<string> dub
        , List<string> subtitle
        , List<string> genres)
    {
        return await _movieRepository.GetAllAdvance(page, limit, name, director, actor, author, dub, subtitle, genres);
    }

    public async Task<movies> GetByIdAsync(string id)
    {
        var movie = await _movieRepository.GetById(id);
        
        if (movie == null)
        {
            throw new MongoException($"Movie with id {id} was not found.");
        }
        
        return movie;
    }

    public async Task<movies> AddAsync(MovieDTO movieDto)
    {
        
        var movie = new movies
        {
            Name = movieDto.Name,
            Director = movieDto.Director,
            Actors = movieDto.Actors,
            Author = movieDto.Author,
            Description = movieDto.Description,
            Dub = movieDto.Dub,
            SubTitle = movieDto.SubTitle,
            Genres = movieDto.Genres,
            Duration = movieDto.Duration,
            Poster = movieDto.Poster
        };
        
        return await _movieRepository.Add(movie);
    }

    public async Task<movies> UpdateAsync(string id, MovieDTO movieDto)
    {
        var movie = await _movieRepository.GetById(id);
        
        if (movie == null)
        {
            throw new MongoException($"Movie with id {id} was not found.");
        }
        
        //lấy ra tất cả các property của MovieDTO
        var dtoProperties = typeof(MovieDTO).GetProperties();
        //lấy ra tất cả các property của movies
        var movieProperties = typeof(movies).GetProperties();
        foreach (var dtoProperty in dtoProperties)
        {
            var newValue = dtoProperty.GetValue(movieDto);

            if (newValue != null)
            {
                var movieProperty = movieProperties.FirstOrDefault(p => p.Name == dtoProperty.Name);
            
                if (movieProperty != null && movieProperty.CanWrite)
                {
                    if (newValue is string strValue && string.IsNullOrEmpty(strValue)) continue;
                
                    // đảm bảo giá trị mới ko bằng giá trị hiện tại trước khi cập nhật
                    var currentValue = movieProperty.GetValue(movie);
                    if (!newValue.Equals(currentValue))
                    {
                        movieProperty.SetValue(movie, newValue);
                    }
                }
            }
        }
        return await _movieRepository.Update(id, movie);
    }

    public async Task<bool> RemoveAsync(string id)
    { 
        return await _movieRepository.Remove(id);
    }
}