using Auth.Domain.Entity;
using Shared.Interfaces;

namespace Auth.Domain.Interfaces;

public interface IResetPasswordRepository : IRepository<ResetPassword>
{
    //kiểm tra token có tồn tại không
    Task<ResetPassword> CheckTokenAsync(string token);
    
    //kiểm tra email có tồn tại không
    Task<ResetPassword> CheckEmailAsync(string email);
}