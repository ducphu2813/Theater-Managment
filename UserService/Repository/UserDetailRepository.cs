using MongoDB.Driver;
using UserService.Context;
using UserService.Entity.Model;
using UserService.Repository.Interface;
using UserService.Repository.MongoDBRepo;

namespace UserService.Repository;

public class UserDetailRepository : MongoDBRepository<UserDetail>, IUserDetailRepository
{
    public UserDetailRepository(MongoDBContext context) : base(context, "UserDetail")
    {
    }
    
    //hàm lấy user detail theo user id
    public async Task<UserDetail> GetByUserId(string userId)
    {
        var filter = Builders<UserDetail>.Filter.Eq("UserId", userId);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
    
}