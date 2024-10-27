using AdminService.Context;
using AdminService.Entity.Model;
using AdminService.Repository.Interface;
using AdminService.Repository.MongoDBRepo;

namespace AdminService.Repository;

public class MovieSaleRepository : MongoDBRepository<MovieSale>, IMovieSaleRepository
{
    public MovieSaleRepository(MongoDBContext context) : base(context, "MovieSale")
    {
    }
    
}