using Auth.Domain.Entity;
using Auth.Domain.Interfaces;
using MongoDB.Driver;
using Shared.Context;
using Shared.Repository;

namespace Auth.Infrastructure.Repository;


public class ResetPasswordRepository : MongoDBRepository<ResetPassword>, IResetPasswordRepository
{
    
    public ResetPasswordRepository(MongoDBContext context) : base(context, "ResetPassword")
    {
    }
    
    //kiểm tra token có tồn tại không
    public async Task<ResetPassword> CheckTokenAsync(string token)
    {
        var filter = Builders<ResetPassword>.Filter.Eq("ResetToken", token);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
    
    //kiểm tra email có tồn tại không
    public async Task<ResetPassword> CheckEmailAsync(string email)
    {
        var filter = Builders<ResetPassword>.Filter.Eq("UserEmail", email);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
    
}