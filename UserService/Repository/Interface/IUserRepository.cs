using UserService.Entity.Model;
using UserService.Repository.MongoDBRepo;

namespace UserService.Repository.Interface;

public interface IUserRepository : IRepository<User>
{
    
}