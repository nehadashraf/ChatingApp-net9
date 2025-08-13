using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChatingApp.Data;
using ChatingApp.Dtos;
using ChatingApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatingApp.Repositories.UserRepo
{
    public class UserRepository(Context db,IMapper mapper) : IUserRepository
    {
        public async Task<AppUser?> GetUserByIdAsync(Guid id)
        {
            return await db.Users.Include(x => x.Photos).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AppUser?> GetUserByUsernameAsync(string username)
        {
            return await db.Users.Include(x => x.Photos).SingleOrDefaultAsync(x=>x.UserName==username);

        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await db.Users
                .Include(x=>x.Photos)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await db.SaveChangesAsync()>0;

        }

        public void Update(AppUser user)
        {
            db.Entry(user).State = EntityState.Modified;
        }

        public async Task <IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await db.Users
                .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                 .ToListAsync();
        }

        public async Task<MemberDto?> GetMembersByIdAsync(Guid id)
        {
            return await db.Users
                .Where(u=>u.Id==id)
                .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                 .SingleOrDefaultAsync();
        }

        public async Task<MemberDto?> GetMembersByUserNameAsync(string username)
        {
            return await db.Users
                .Where(u => u.UserName == username)
                .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

      
    }
}
