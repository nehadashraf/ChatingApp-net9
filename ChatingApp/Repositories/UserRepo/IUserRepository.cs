using ChatingApp.Dtos;
using ChatingApp.Models;

namespace ChatingApp.Repositories.UserRepo
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser?> GetUserByIdAsync(Guid id);
        Task<AppUser?> GetUserByUsernameAsync(string username);
        Task<IEnumerable<MemberDto>> GetMembersAsync();
        Task<MemberDto?> GetMembersByIdAsync(Guid id);
        Task<MemberDto?> GetMembersByUserNameAsync(string username);
    }
}
