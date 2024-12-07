using Auth.Application.Interfaces;
using Auth.Domain.DTO;
using Auth.Domain.Entity;
using Auth.Domain.Exception;
using Auth.Domain.Interfaces;
using Auth.Infrastructure.Token;

namespace Auth.Application.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly TokenProvider _tokenProvider;
    private readonly IRoleRepository _roleRepository;
    private readonly IResetPasswordRepository _resetPasswordRepository;
    private readonly string RandomChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private readonly IEmailService _emailService;
    
    public UserService(IUserRepository userRepository,
        TokenProvider tokenProvider,
        IRoleRepository roleRepository,
        IResetPasswordRepository resetPasswordRepository,
        IEmailService emailService)
    {
        _userRepository = userRepository;
        _tokenProvider = tokenProvider;
        _roleRepository = roleRepository;
        _resetPasswordRepository = resetPasswordRepository;
        _emailService = emailService;
    }
    
    public async Task<Dictionary<string, object>> GetAllAsync(int page, int limit)
    {
        return await _userRepository.GetAll(page, limit);
    }

    public async Task<User> GetByIdAsync(string id)
    {
        var user = await _userRepository.GetById(id);
        
        if (user == null)
        {
            throw new NotFoundException($"User with id {id} was not found.");
        }
        
        return user;
    }
    
    //lấy user theo username
    public async Task<User> GetUserByUsername(string username)
    {
        var user = await _userRepository.GetUserByUsername(username);
        
        if (user == null)
        {
            throw new NotFoundException($"User with username {username} was not found.");
        }
        
        return user;
    }

    public async Task<User> AddAsync(User user)
    {
        return await _userRepository.Add(user);
    }

    public async Task<User> UpdateAsync(string id, User user)
    {
        var existingUser = await _userRepository.GetById(id);
        
        if (existingUser == null)
        {
            throw new NotFoundException($"User with id {id} was not found.");
        }
        
        return await _userRepository.Update(id, user);
    }

    public async Task<bool> RemoveAsync(string id)
    {
        var existingUser = await _userRepository.GetById(id);
        
        if (existingUser == null)
        {
            throw new NotFoundException($"User with id {id} was not found.");
        }
        
        return await _userRepository.Remove(id);
    }
    
    //hàm login
    public async Task<string> LoginAsync(string username, string password)
    {
        var user = await _userRepository.LoginAsync(username, password);
        
        if(user == null)
        {
            return "unauthorized";
        }
        
        if(user.isEmailVerified == "no")
        {
            return "notmailverified";
        }
        
        return _tokenProvider.Create(user);
    }
    
    //hàm register
    public async Task<User> RegisterAsync(RegisterRequest registerRequest)
    {
        var user = await _userRepository.GetUserByUsername(registerRequest.Username);
        
        if(user != null)
        {
            throw new InvalidOperationException($"User with username {registerRequest.Username} already exists.");
        }
        
        var emailUser = await _userRepository.GetUserByEmail(registerRequest.Email);
        
        if(emailUser != null)
        {
            throw new InvalidOperationException($"User with email {registerRequest.Email} already exists.");
        }
        
        User newUser = new User
        {
            Username = registerRequest.Username,
            Password = registerRequest.Password,
            Email = registerRequest.Email,
            Roles = registerRequest.Roles,
            Departments = registerRequest.Departments,
            isEmailVerified = "no"
        };

        newUser = await _userRepository.Add(newUser);
        
        //tạo mail token dựa trên user id + ngày hiện tại
        var mailToken = newUser.Id + DateTime.Now.ToString("yyyyMMddHHmmss");
        
        newUser.confirmMailToken = mailToken;
        
        return await _userRepository.Update(newUser.Id, newUser);
    }
    
    //lấy tất cả role permission của user theo username
    public async Task<List<RolePermission>> GetRolePermissionsByUsernameAsync(string username)
    {
        var user = await _userRepository.GetUserByUsername(username);
        
        if(user == null)
        {
            throw new NotFoundException($"User with username {username} was not found.");
        }
        
        return await _roleRepository.GetRolePermissionByRolename(user.Roles);
    }
    
    //lấy tất cả user theo username và role
    public async Task<Dictionary<string, object>> GetAllAdvance(int page, int limit, string username, List<string> roles)
    {
        return await _userRepository.GetAllAdvance(page, limit, username, roles);
    }
    
    //tìm user theo email
    public async Task<User> GetUserByEmail(string email)
    {
        return await _userRepository.GetUserByEmail(email);
    }
    
    //hàm xác nhận mail
    public async Task<User> ConfirmEmailAsync(string email, string token)
    {
        var user = await _userRepository.GetUserByEmail(email);
        
        if(user == null)
        {
            Console.WriteLine("User not found with mail " + email);
            throw new NotFoundException($"User with email {email} was not found.");
        }
        
        if(user.confirmMailToken != token)
        {
            Console.WriteLine("Invalid token.");
            throw new InvalidOperationException("Invalid token.");
        }
        
        if(user.isEmailVerified == "confirmed")
        {
            Console.WriteLine("Email already confirmed.");
            throw new InvalidOperationException("Email already confirmed.");
        }
        
        user.isEmailVerified = "confirmed";
        user.confirmMailToken = "";
        
        return await _userRepository.Update(user.Id, user);
    }
    
    //quên mật khẩu, gửi mail xác nhận
    public async Task<User> ForgotPasswordAsync(string email)
    {
        var user = await _userRepository.GetUserByEmail(email);
        
        if(user == null)
        {
            throw new NotFoundException($"User with email {email} was not found.");
        }
        
        //kiểm tra email có tồn tại trong ResetPassword chưa
        var resetPassword = await _resetPasswordRepository.CheckEmailAsync(email);

        if (resetPassword != null)
        {
            throw new InvalidOperationException($"Reset password request for email {email} already exists.");
        }
        
        //tạo 1 token 6 ký tự ngẫu nhiên và không trùng với token khác
        while(true)
        {
            var random = new Random();
            string token = new string(Enumerable.Repeat(RandomChars, 6)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
            var checkToken = await _resetPasswordRepository.CheckTokenAsync(token);
            
            //token không trùng
            if(checkToken == null)
            {
                resetPassword = new ResetPassword
                {
                    UserEmail = email,
                    ResetToken = token,
                    ExpiryTime = DateTime.UtcNow.AddMinutes(5)
                };
                //lưu token vào db
                await _resetPasswordRepository.Add(resetPassword);
                //gửi mail
                await _emailService.SendEmailAsync(email, "Reset Password", $"Your reset password code for account {user.Username} is: " + "<strong>"+ token+ "</strong>");
                break;
            }
        }
        return user;
    }
    
    //đổi mật khẩu của quên mật khẩu
    public async Task<User> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest)
    {
        //lấy token ra và kiểm tra
        var resetPassword = await _resetPasswordRepository.CheckTokenAsync(resetPasswordRequest.token);
        
        if(resetPassword == null)
        {
            throw new NotFoundException("Token not found.");
        }
        
        //kiểm tra token đã hết hạn chưa
        if(resetPassword.ExpiryTime < DateTime.UtcNow)
        {
            //nếu hết hạn thì xóa luôn token
            await _resetPasswordRepository.Remove(resetPassword.Id);
            throw new InvalidOperationException("Token expired.");
        }
        
        //lấy user ra và đổi mật khẩu
        var user = await _userRepository.GetUserByEmail(resetPassword.UserEmail);
        
        if(user == null)
        {
            throw new NotFoundException($"User with email {resetPassword.UserEmail} was not found.");
        }

        //cập nhật pass
        user.Password = resetPasswordRequest.newPassword;
        await _userRepository.Update(user.Id, user);
        //xóa token
        await _resetPasswordRepository.Remove(resetPassword.Id);
        
        return user;
    }
}