using UserService.Context;
using UserService.Entity.Model;
using UserService.Repository.Interface;
using UserService.Repository.MongoDBRepo;

namespace UserService.Repository;

public class UserRepository : MongoDBRepository<User>, IUserRepository
{
    public UserRepository(MongoDBContext context) : base(context, "User")
    {
    }
    
}