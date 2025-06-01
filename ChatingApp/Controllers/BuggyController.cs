using ChatingApp.Data;
using ChatingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatingApp.Controllers
{
    public class BuggyController(Context db) : BaseApiController
    {
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetAuth()
        {
            return "Secert text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = db.Users.Find(-1);
            if(thing ==null) return NotFound();
            return thing;
        }
        [HttpGet("server-error")]
        public ActionResult<AppUser> GetServerError()
        {

            var thing =db.Users.Find(-1)?? throw new Exception ("Abad thing happened");
            return thing;

        }
        // [HttpGet("bad-request")]
        //public ActionResult<AppUser> GetServerError()
        //{

        //    var thing = db.Users.Find(-1) ?? throw new Exception("Abad thing happened");
        //    return thing;

        //}

    }
}
