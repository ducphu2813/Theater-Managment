using AdminService.Entity.Model;
using AdminService.Exception;
using AdminService.Repository.Interface;
using AdminService.Service.Interface;

namespace AdminService.Service;

public class MovieSaleService : IMovieSaleService
{
    private readonly IMovieSaleRepository _movieSaleRepository;
    
    public MovieSaleService(IMovieSaleRepository movieSaleRepository)
    {
        _movieSaleRepository = movieSaleRepository;
    }
    
    public async Task<IEnumerable<MovieSale>> GetAllMovieSaleAsync()
    {
        return await _movieSaleRepository.GetAll();
    }
    
    //tìm nâng cao
    public async Task<Dictionary<string, object>> GetAllAdvance(
        int page
        , int limit
        , List<string> movieId
        , List<string> genres
        , DateTime fromCreateDate
        , DateTime toCreateDate
        , float fromTotalPrice
        , float toTotalPrice
        , string sortByCreateDate
        , string sortByTotalPrice)
    {
        var results = await _movieSaleRepository.GetAllAdvance(
            page
            , limit
            , movieId
            , genres
            , fromCreateDate
            , toCreateDate
            , fromTotalPrice
            , toTotalPrice
            , sortByCreateDate
            , sortByTotalPrice);
        
        //chỉnh timezone cho từng item
        var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh");
        foreach (var item in results["records"] as List<MovieSale>)
        {
            item.TicketCreatedDate = item.TicketCreatedDate.HasValue
                ? TimeZoneInfo.ConvertTimeFromUtc(item.TicketCreatedDate.Value, timeZoneInfo)
                : (DateTime?)null;
            
            item.ShowTime = item.ShowTime.HasValue
                ? TimeZoneInfo.ConvertTimeFromUtc(item.ShowTime.Value, timeZoneInfo)
                : (DateTime?)null;
        }
        return results;
    }

    public async Task<MovieSale> GetMovieSaleByIdAsync(string id)
    {
        var movieSale = await _movieSaleRepository.GetById(id);
        
        if (movieSale == null)
        {
            throw new NotFoundException($"Movie Sale with id {id} was not found.");
        }
        
        return movieSale;
    }

    public async Task<MovieSale> AddMovieSaleAsync(MovieSale movieSale)
    {
        return await _movieSaleRepository.Add(movieSale);
    }

    public async Task<MovieSale> UpdateMovieSaleAsync(string id, MovieSale movieSale)
    {
        var existingMovieSale = await _movieSaleRepository.GetById(id);
        
        if (existingMovieSale == null)
        {
            throw new NotFoundException($"Movie Sale with id {id} was not found.");
        }
        
        return await _movieSaleRepository.Update(id, movieSale);
    }

    public async Task<bool> RemoveMovieSaleAsync(string id)
    {
        var existingMovieSale = await _movieSaleRepository.GetById(id);
        
        if (existingMovieSale == null)
        {
            throw new NotFoundException($"Movie Sale with id {id} was not found.");
        }
        
        return await _movieSaleRepository.Remove(id);
    }
    
    //xóa tất cả movie sale
    public async Task DeleteAll()
    {
        await _movieSaleRepository.DeleteAll();
    }
    
    //tìm theo payment id
    public async Task<MovieSale> GetByPaymentId(string paymentId)
    {
        return await _movieSaleRepository.GetByPaymentId(paymentId);
    }
    
    //tìm theo user id
    public async Task<List<MovieSale>> GetByUserId(string userId)
    {
        return await _movieSaleRepository.GetByUserId(userId);
    }
    
    //tìm theo 1 khoảng cách ngày, tìm theo TicketCreatedDate
    public async Task<List<MovieSale>> FindByDateRangeAsync(DateTime from, DateTime to)
    {
        //chuyển về utc
        var fromUtc = DateTime.SpecifyKind(from, DateTimeKind.Utc);
        var toUtc = DateTime.SpecifyKind(to, DateTimeKind.Utc);
        
        return await _movieSaleRepository.FindByDateRangeAsync(fromUtc, toUtc);
    }
}