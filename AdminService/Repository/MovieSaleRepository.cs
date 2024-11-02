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
    
}