using AdminService.Context;
using AdminService.Entity.Model;
using AdminService.Repository.Interface;
using AdminService.Repository.MongoDBRepo;
using MongoDB.Driver;

namespace AdminService.Repository;

public class MovieSaleRepository : MongoDBRepository<MovieSale>, IMovieSaleRepository
{
    public MovieSaleRepository(MongoDBContext context) : base(context, "MovieSale")
    {
    }
    
    //xóa tất cả movie sale
    public async Task DeleteAll()
    {
        await _collection.DeleteManyAsync(Builders<MovieSale>.Filter.Empty);
    }
    
    //tìm theo payment id
    public async Task<MovieSale> GetByPaymentId(string paymentId)
    {
        return await _collection.Find(x => x.PaymentId == paymentId).FirstOrDefaultAsync();
    }
    
    //tìm theo user id
    public async Task<List<MovieSale>> GetByUserId(string userId)
    {
        return await _collection.Find(x => x.UserId == userId).ToListAsync();
    }
    
    //tìm theo 1 khoảng cách ngày, tìm theo TicketCreatedDate
    public async Task<List<MovieSale>> FindByDateRangeAsync(DateTime from, DateTime to)
    {
        // chuyển về utc
        // var fromUtc = from.Date.ToUniversalTime(); // lấy thời gian bắt đầu từ 00:00:00.000 utc
        // var toUtc = to.Date.AddDays(1).AddTicks(-1).ToUniversalTime(); // lấy thời gian kết thúc là 23:59:59.999 utc

        // tạo 1 filter để tìm theo TicketCreatedDate nằm trong khoảng từ fromUtc đến toUtc
        var filter = Builders<MovieSale>.Filter.And(
            Builders<MovieSale>.Filter.Gte(s => s.TicketCreatedDate, from),
            Builders<MovieSale>.Filter.Lte(s => s.TicketCreatedDate, to)
        );

        // tìm theo filter vừa tạo
        return await _collection.Find(filter).ToListAsync();
    }

    
}