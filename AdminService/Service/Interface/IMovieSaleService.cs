using AdminService.Entity.Model;

namespace AdminService.Service.Interface;

public interface IMovieSaleService
{
    Task<IEnumerable<MovieSale>> GetAllMovieSaleAsync();
    Task<MovieSale> GetMovieSaleByIdAsync(string id);
    Task<MovieSale> AddMovieSaleAsync(MovieSale movieSale);
    Task<MovieSale> UpdateMovieSaleAsync(string id, MovieSale movieSale);
    Task<bool> RemoveMovieSaleAsync(string id);
    
    //xóa tất cả movie sale
    Task DeleteAll();
    
}