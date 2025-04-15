using ChatingApp.Data;
using ChatingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(Context db) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users=await db.Users.ToListAsync();
            return Ok(users);
        }
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
