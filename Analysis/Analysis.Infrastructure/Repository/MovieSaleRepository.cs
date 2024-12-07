using Analysis.Domain.Entity;
using Analysis.Domain.Interfaces;
using MongoDB.Driver;
using Shared.Context;
using Shared.Repository;

namespace Analysis.Infrastructure.Repository;

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
        //tạo filter
        var filters = new List<FilterDefinition<MovieSale>>();
        
        //kiểm tra param movieId
        if (movieId != null && movieId.Count > 0)
        {
            filters.Add(Builders<MovieSale>.Filter.In(u => u.MovieDetail.Id, movieId));
        }
        
        //kiểm tra param genres
        if (genres != null && genres.Count > 0)
        {
            filters.Add(Builders<MovieSale>.Filter.AnyIn(u => u.Genres, genres));
        }
        
        //kiểm tra param fromCreateDate
        if (fromCreateDate != DateTime.MinValue)
        {
            filters.Add(Builders<MovieSale>.Filter.Gte(u => u.TicketCreatedDate, fromCreateDate));
        }
        
        //kiểm tra param toCreateDate
        if (toCreateDate != DateTime.MinValue)
        {
            filters.Add(Builders<MovieSale>.Filter.Lte(u => u.TicketCreatedDate, toCreateDate));
        }
        
        //kiểm tra param fromTotalPrice
        if (fromTotalPrice != 0)
        {
            filters.Add(Builders<MovieSale>.Filter.Gte(u => u.TotalAmount, fromTotalPrice));
        }
        
        //kiểm tra param toTotalPrice
        if (toTotalPrice != 0)
        {
            filters.Add(Builders<MovieSale>.Filter.Lte(u => u.TotalAmount, toTotalPrice));
        }
        
        //kết hợp các filter
        var combinedFilter = filters.Count > 0 ? Builders<MovieSale>.Filter.And(filters) : Builders<MovieSale>.Filter.Empty;
        
        //tạo sort definition
        SortDefinition<MovieSale> sortDefinition = Builders<MovieSale>.Sort.Descending(u => u.TicketCreatedDate);
        //kiểm tra param sortByCreateDate
        if(sortByCreateDate == "asc")
        {
            sortDefinition = Builders<MovieSale>.Sort.Ascending(u => u.TicketCreatedDate);
        }
        else if(sortByCreateDate == "desc")
        {
            sortDefinition = Builders<MovieSale>.Sort.Descending(u => u.TicketCreatedDate);
        }
        //kiểm tra param sortByTotalPrice
        if(sortByTotalPrice == "asc")
        {
            sortDefinition = Builders<MovieSale>.Sort.Ascending(u => u.TotalAmount);
        }
        else if(sortByTotalPrice == "desc")
        {
            sortDefinition = Builders<MovieSale>.Sort.Descending(u => u.TotalAmount);
        }
        //lấy count record
        var totalRecords = await _collection.CountDocumentsAsync(combinedFilter);
        //tìm và sắp xếp
        var records = await _collection
            .Find(combinedFilter) //sử dụng filter
            .Sort(sortDefinition) //sắp xếp
            .Skip((page - 1) * limit)
            .Limit(limit)
            .ToListAsync();
        
        //gộp thành 1 dictionary
        return new Dictionary<string, object>
        {
            {"totalRecords", totalRecords},
            {"records", records},
            {"limit", limit},
            {"page", page}
        };
    }


}