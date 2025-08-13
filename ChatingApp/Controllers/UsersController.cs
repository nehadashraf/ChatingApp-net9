using AutoMapper;
using ChatingApp.Dtos;
using ChatingApp.Extentions;
using ChatingApp.Interfaces;
using ChatingApp.Models;
using ChatingApp.Repositories.UserRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatingApp.Controllers
{
    [Authorize]
    public class UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await userRepository.GetMembersAsync();
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
        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdatedDto memberUpdatedDto)
        {
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user is null)
            {
                return NotFound("User not found");
            }
            mapper.Map(memberUpdatedDto, user);
            if (await userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Faild to update the user");

        }
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user is null)
            {
                return NotFound("User not found");
            }
            var result = await photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);
            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };
            user.Photos.Add(photo);
            if (await userRepository.SaveAllAsync())
                return CreatedAtAction(nameof(GetUsers), new { username = user.UserName }, mapper.Map<PhotoDto>(photo));
            return BadRequest("Problem adding photo");
        }
        [HttpPut("set-main-photo/{photoId:int}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user is null)
            {
                return NotFound("User not found");
            }
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo is null)
            {
                return NotFound("Photo not found");
            }
            if (photo.IsMain)
            {
                return BadRequest("This is already your main photo");
            }
            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain is not null)
            {
                currentMain.IsMain = false;
            }
            photo.IsMain = true;
            if (await userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Problem setting main photo");

        }
        [HttpDelete("delete-photo/{photoId:int}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user=await userRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user is null)
            {
                return NotFound("User not found");
            }
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo is null)
            {
                return NotFound("Photo not found");
            }
            if (photo.IsMain)
            {
                return BadRequest("This is photo can't be deleted");
            }
            if (photo.PublicId != null)
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);
            }
            user.Photos.Remove(photo);
            if( await  userRepository.SaveAllAsync()) { return Ok(); }
            return BadRequest("problem deleting photo");

        }
    }
}
