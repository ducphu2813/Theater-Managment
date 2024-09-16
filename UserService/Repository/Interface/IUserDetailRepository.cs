using UserService.Entity.Model;
using UserService.Repository.MongoDBRepo;

namespace UserService.Repository.Interface;

public interface IUserDetailRepository : IRepository<UserDetail>
{
    //hàm lấy user detail theo user id
    Task<UserDetail> GetByUserId(string userId);
}