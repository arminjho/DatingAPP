using System.Security.Claims;
using System.Text.RegularExpressions;
using AutoMapper;
using DatingWebApp.Data;
using DatingWebApp.DTOs;
using DatingWebApp.Entities;
using DatingWebApp.Extensions;
using DatingWebApp.Helpers;
using DatingWebApp.Interfaces;
using DatingWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace DatingWebApp.Controllers
{
    [Authorize]
    public class UsersController(IUnitOfWork unitOfWork,
        IMapper mapper, IPhotoService photoService) : BaseApiController
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
        {
            userParams.CurrentUsername = User.GetUsername();
            var users = await unitOfWork.UserRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(users);

            return Ok(users);
        }

        [HttpGet("{username}")]  
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var currentUsername = User.GetUsername();
            var user = await unitOfWork.UserRepository.GetMemberAsync(username,
                isCurrentUser: currentUsername == username);

            if (user == null) return NotFound();

            return user;
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (username == null) { return BadRequest("No suername found in token"); }
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            if (user == null) return BadRequest("Could not find user");
            mapper.Map(memberUpdateDto, user);
            if (await unitOfWork.Complete()) return NoContent();
            return BadRequest("Failed to update the user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return BadRequest("Cannot update user");

            var result = await photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            

            user.Photos.Add(photo);

            if (await unitOfWork.Complete())
            {
                return CreatedAtAction(nameof(GetUser), new { user = user.UserName }, mapper.Map<PhotoDto>(photo));
            }


            return BadRequest("Problem addding photo");
        }


        [HttpPost("add-photo-with-tags")]
        public async Task<ActionResult<PhotoWithTagsDto>> AddPhotoWithTags(IFormFile file,
            [FromQuery] List<string> tags)
        {

            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null) { return BadRequest("Cannot update user"); }
            var result = await photoService.AddPhotoAsync(file);

            if (result.Error != null){ return BadRequest(result.Error.Message); }
            var photo = new Photo
            {

                Url = result.SecureUrl.AbsoluteUri,

                PublicId = result.PublicId

            };

            var validTags = tags
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(t => t.Trim().ToLower())
                .Distinct()
                .ToList();

            foreach (var tagName in validTags)
            {

                if (!Regex.IsMatch(tagName, @"^[a-zA-Z0-9\s\-]{2,30}$"))

                    return BadRequest($"Invalid tag: {tagName}");

            }

            foreach (var tagName in validTags)
            {

                var tag = await unitOfWork.TagRepository.GetOrCreateTagAsync(tagName);

                photo.PhotoTags.Add(new PhotoTag { Photo = photo, Tag = tag });

            }

            user.Photos.Add(photo);

            if (await unitOfWork.Complete())
            {

                return CreatedAtAction(nameof(GetUser),

                    new { user = user.UserName },

                    mapper.Map<PhotoWithTagsDto>(photo));

            }

            return BadRequest("Problem adding photo");
        }



         [HttpPut("set-main-photo/{photoId:int}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null) return BadRequest("Could not find user");
            var photo=user.Photos.FirstOrDefault(x=>x.Id==photoId);
            if (photo == null || photo.IsMain) return BadRequest("Cannot use this as main photo");
            var currentMain=user.Photos.FirstOrDefault(x=>x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;    
            if(await unitOfWork.Complete()) return NoContent();

            return BadRequest("Problem setting main photo");

        }


        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return BadRequest("User not found");


            var photo = await unitOfWork.PhotoRepository.GetPhotoById(photoId);

            if (photo == null || photo.IsMain) return BadRequest("This photo cannot be deleted");


            if (photo.PublicId != null)
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to delete the photo");
        }

    }
}
