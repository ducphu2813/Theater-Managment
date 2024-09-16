using UserService.Entity.Model;
using UserService.Exceptions;
using UserService.Repository.Interface;
using UserService.Service.Interface;

namespace UserService.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
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
}