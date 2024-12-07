using Analysis.Domain.Entity;

namespace Analysis.Application.Interfaces;

public interface IMovieSaleService
{
    Task<Dictionary<string, object>> GetAllAsync(int page, int limit);
    Task<MovieSale> GetMovieSaleByIdAsync(string id);
    Task<MovieSale> AddMovieSaleAsync(MovieSale movieSale);
    Task<MovieSale> UpdateMovieSaleAsync(string id, MovieSale movieSale);
    Task<bool> RemoveMovieSaleAsync(string id);
    
    //xóa tất cả movie sale
    Task DeleteAll();
    
    //tìm theo payment id
    Task<MovieSale> GetByPaymentId(string paymentId);
    
    //tìm theo user id
    Task<List<MovieSale>> GetByUserId(string userId);
    
    //tìm theo 1 khoảng cách ngày, tìm theo TicketCreatedDate
    Task<List<MovieSale>> FindByDateRangeAsync(DateTime from, DateTime to);
    
    //tìm nâng cao
    Task<Dictionary<string, object>> GetAllAdvance(
        int page
        , int limit
        , List<string> movieId
        , List<string> genres
        , DateTime fromCreateDate
        , DateTime toCreateDate
        , float fromTotalPrice
        , float toTotalPrice
        , string sortByCreateDate
        , string sortByTotalPrice);
    
}