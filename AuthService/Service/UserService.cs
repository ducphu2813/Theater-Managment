using AuthService.Entity.Model;
using AuthService.Exceptions;
using AuthService.Repository.Interface;
using AuthService.Service.Interface;
using AuthService.Token;

namespace AuthService.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly TokenProvider _tokenProvider;
    private readonly IRoleRepository _roleRepository;
    
    public UserService(IUserRepository userRepository,
        TokenProvider tokenProvider,
        IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _tokenProvider = tokenProvider;
        _roleRepository = roleRepository;
    }
    
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _userRepository.GetAll();
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
        
        return _tokenProvider.Create(user);
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
}