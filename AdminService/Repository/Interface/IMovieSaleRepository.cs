using AdminService.Entity.Model;
using AdminService.Repository.MongoDBRepo;

namespace AdminService.Repository.Interface;

public interface IMovieSaleRepository : IRepository<MovieSale>
{
    //xóa tất cả movie sale
    Task DeleteAll();
    
    //tìm theo payment id
    Task<MovieSale> GetByPaymentId(string paymentId);
    
    //tìm theo user id
    Task<List<MovieSale>> GetByUserId(string userId);
    
    //tìm theo 1 khoảng cách ngày, tìm theo TicketCreatedDate
    Task<List<MovieSale>> FindByDateRangeAsync(DateTime from, DateTime to);
}