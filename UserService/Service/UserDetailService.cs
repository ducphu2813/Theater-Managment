using UserService.Entity.Model;
using UserService.Exceptions;
using UserService.Repository.Interface;
using UserService.Service.Interface;

namespace UserService.Service;

public class UserDetailService : IUserDetailService
{
    private readonly IUserDetailRepository _userDetailRepository;

    public UserDetailService(IUserDetailRepository userDetailRepository)
    {
        _userDetailRepository = userDetailRepository;
    }

    public async Task<IEnumerable<UserDetail>> GetAllAsync()
    {
        return await _userDetailRepository.GetAll();
    }

    public async Task<UserDetail> GetByIdAsync(string id)
    {
        return await _userDetailRepository.GetById(id) ?? throw new NotFoundException($"UserDetail with id {id} was not found.");
    }
    
    public async Task<UserDetail> GetByUserIdAsync(string userId)
    {
        return await _userDetailRepository.GetByUserId(userId) ?? throw new NotFoundException($"UserDetail with user id {userId} was not found.");
    }
    
    public async Task<UserDetail> AddAsync(UserDetail userDetail)
    {
        return await _userDetailRepository.Add(userDetail);
    }

    public async Task<UserDetail> UpdateAsync(string id, UserDetail userDetail)
    {
        UserDetail existingUserDetail = await _userDetailRepository.GetById(id);
        
        if (existingUserDetail == null)
        {
            throw new NotFoundException($"UserDetail with id {id} was not found.");
        }
        
        return await _userDetailRepository.Update(id, userDetail);
    }

    public async Task<bool> RemoveAsync(string id)
    {
        return await _userDetailRepository.Remove(id);
    }
    
}