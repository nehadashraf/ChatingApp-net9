using ChatingApp.Data;
using ChatingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatingApp.Controllers
{
    public class UsersController(Context db) : BaseApiController
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users=await db.Users.ToListAsync();
            return Ok(users);
        }
        [Authorize]
        [HttpGet("{Id:guid}")]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(Guid Id)
        {
            var user = await db.Users.FindAsync(Id);
            if(user is null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
