using AuthService.Entity.Model;
using AuthService.Repository.MongoDBRepo;

namespace AuthService.Repository.Interface;

public interface IResetPasswordRepository : IRepository<ResetPassword>
{
    //kiểm tra token có tồn tại không
    Task<ResetPassword> CheckTokenAsync(string token);
    
    //kiểm tra email có tồn tại không
    Task<ResetPassword> CheckEmailAsync(string email);
}