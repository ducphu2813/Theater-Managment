using MovieService.DTO;
using MovieService.Entity.Model;
using MovieService.Exceptions;
using MovieService.Repository.Interface;
using MovieService.Service.Interface;

namespace MovieService.Service;

public class MovieService : IMovieService
{
    
    private readonly IMovieRepository _movieRepository;
    
    public MovieService(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }
    
    public async Task<IEnumerable<movies>> GetAllAsync()
    {
        return await _movieRepository.GetAll();
    }

    public async Task<movies> GetByIdAsync(string id)
    {
        var movie = await _movieRepository.GetById(id);
        
        if (movie == null)
        {
            throw new NotFoundException($"Movie with id {id} was not found.");
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
            Genres = movieDto.Genres
        };
        
        return await _movieRepository.Add(movie);
    }

    public async Task<movies> UpdateAsync(string id, MovieDTO movieDto)
    {
        var movie = await _movieRepository.GetById(id);
        
        if (movie == null)
        {
            throw new NotFoundException($"Movie with id {id} was not found.");
        }
        
        //lấy ra tất cả các property của MovieDTO
        var dtoProperties = typeof(MovieDTO).GetProperties();
        //lấy ra tất cả các property của movies
        var movieProperties = typeof(movies).GetProperties();
        foreach (var dtoProperty in dtoProperties)
        {
            var newValue = dtoProperty.GetValue(movieDto); // Lấy giá trị từ movieDto

            if (newValue != null)
            {
                var movieProperty = movieProperties.FirstOrDefault(p => p.Name == dtoProperty.Name);
            
                if (movieProperty != null && movieProperty.CanWrite)
                {
                    if (newValue is string strValue && string.IsNullOrEmpty(strValue)) continue;
                
                    // Đảm bảo giá trị mới không bằng giá trị hiện tại trước khi cập nhật
                    var currentValue = movieProperty.GetValue(movie);
                    if (!newValue.Equals(currentValue))
                    {
                        movieProperty.SetValue(movie, newValue); // Cập nhật giá trị mới vào đối tượng movie
                    }
                }
            }
        }
        
        // movie.Name = movieDto.Name;
        // movie.Director = movieDto.Director;
        // movie.Actors = movieDto.Actors;
        // movie.Author = movieDto.Author;
        // movie.Description = movieDto.Description;
        // movie.Dub = movieDto.Dub;
        // movie.SubTitle = movieDto.SubTitle;
        // movie.Genres = movieDto.Genres;
        
        return await _movieRepository.Update(id, movie);
    }

    public async Task<bool> RemoveAsync(string id)
    { 
        return await _movieRepository.Remove(id);
    }
}