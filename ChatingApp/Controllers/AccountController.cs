using AutoMapper;
using Azure.Identity;
using ChatingApp.Data;
using ChatingApp.Dtos;
using ChatingApp.Interfaces;
using ChatingApp.Models;
using ChatingApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ChatingApp.Controllers
{
    public class AccountController(Context db,ITokenService tokenService,IMapper mapper) : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username))
            {
                return BadRequest("Username is already exist");
            }
            using var hmac = new HMACSHA512();
            var user = mapper.Map<AppUser>(registerDto);
            user.UserName = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;
          
            db.Add(user);
            await db.SaveChangesAsync();
            return Ok(new UserDto
            {
                Username = user.UserName,
                Token = tokenService.CreateToken(user),
                KnownAs=user.KnownAs

            });
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await db.Users.Include(x => x.Photos).FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
            if (user == null) return Unauthorized("Invalid Username");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computedhash.Length; i++)
            {
                if (computedhash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }
             return Ok(new UserDto
            {
                Username = user.UserName,
                Token = tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs

             }); 
        }
        private async Task<bool> UserExists(string username)
        {
            return await db.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
        }


    }
}
