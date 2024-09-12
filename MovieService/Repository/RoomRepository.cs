using MovieService.Context;
using MovieService.Entity.Model;
using MovieService.Repository.Interface;
using MovieService.Repository.MongoDBRepo;

namespace MovieService.Repository;

public class RoomRepository : MongoDBRepository<Room>, IRoomRepository
{
    public RoomRepository(MongoDBContext context) : base(context, "Room")
    {
    }
    
}