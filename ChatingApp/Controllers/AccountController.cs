﻿using Azure.Identity;
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
    public class AccountController(Context db,ITokenService tokenService) : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username))
            {
                return BadRequest("Username is already exist");
            }
            return Ok();
          //using var hmac = new HMACSHA512();
          //  var user = new AppUser
          //  {

          //      UserName = registerDto.Username.ToLower(),
          //      PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
          //      PasswordSalt = hmac.Key
          //  };
          //  db.Add(user);
          //  await db.SaveChangesAsync();
          //  return Ok(new UserDto
          //  {
          //      Username = user.UserName,
          //      Token = tokenService.CreateToken(user)

          //  });
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
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
                Token = tokenService.CreateToken(user)

            }); ;
        }
        private async Task<bool> UserExists(string username)
        {
            return await db.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
        }


    }
}
