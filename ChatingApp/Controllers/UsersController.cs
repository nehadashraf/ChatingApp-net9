using AutoMapper;
using ChatingApp.Dtos;
using ChatingApp.Models;
using ChatingApp.Repositories.UserRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ChatingApp.Controllers
{
    [Authorize]
    public class UsersController(IUserRepository userRepository) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users= await userRepository.GetMembersAsync();
            return Ok(users);
        }
        [HttpGet("{Id:guid}")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers(Guid Id)
        {
            var user = await userRepository.GetMembersByIdAsync(Id);
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet("{username}")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers(string username)
        {
            var user = await userRepository.GetMembersByUserNameAsync(username);
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
