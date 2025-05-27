using System.Text.RegularExpressions;
using AutoMapper;
using DatingWebApp.DTOs;
using DatingWebApp.Entities;
using DatingWebApp.Helpers;
using DatingWebApp.Interfaces;
using DatingWebApp.Middleware;
using DatingWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingWebApp.Controllers
{
    public class AdminController(UserManager<AppUser> userManager, IUnitOfWork unitOfWork,
        IPhotoService photoService, ILogger<ExceptionMiddleware> logger, IMapper mapper) : BaseApiController
    {
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await userManager.Users
                .OrderBy(x => x.UserName)
                .Select(x => new
                {
                    x.Id,
                    Username = x.UserName,
                    Roles = x.UserRoles.Select(x => x.Role.Name).ToList()
                }).ToListAsync();
            return Ok(users);
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, string roles)
        {
            if (string.IsNullOrEmpty(roles)) { return BadRequest("you must select at least one role"); }
            var selectedRoles = roles.Split(',').ToArray();

            var user = await userManager.FindByNameAsync(username);

            if (user == null) { return NotFound("User not found"); }

            var userRoles = await userManager.GetRolesAsync(user);

            var result = await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) { return BadRequest("Failed to add to roles"); }

            result = await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) { return BadRequest("Failed to remove from roles"); }

            return Ok(await userManager.GetRolesAsync(user));
        }

      


        [Authorize(Policy = "RequireAdminRole")]

        [HttpGet("tags")]

        public async Task<ActionResult<IEnumerable<TagDto>>> GetAllTags()

        {

            var tags = await unitOfWork.TagRepository.GetAllTagsAsync();

            return Ok(mapper.Map<IEnumerable<TagDto>>(tags));

        }



        [Authorize(Policy = "RequireAdminRole")]

        [HttpPost("add-tag")]

        public async Task<ActionResult<TagDto>> AddTag([FromBody] TagDto tagDto)
        {
            if (string.IsNullOrWhiteSpace(tagDto.Name))
               { return BadRequest("Tag name is required."); }

            var tagName = tagDto.Name.Trim();

            if (!Regex.IsMatch(tagName, @"^[a-zA-Z0-9\s\-]{2,30}$"))
              { return BadRequest("Tag name contains invalid characters or is too short/long."); }

            var existingTags = await unitOfWork.TagRepository.GetAllTagsAsync();

            if (existingTags.Any(t => t.Name.Equals(tagName, StringComparison.OrdinalIgnoreCase)))
               { return BadRequest("A tag with the same name already exists."); }

            var tag = new Tag { Name = tagName };

            await unitOfWork.TagRepository.AddTagAsync(tag);

            return Ok(mapper.Map<TagDto>(tag));

        }



        [Authorize(Policy = "RequireAdminRole")]

        [HttpDelete("delete-tag/{id:int}")]

        public async Task<ActionResult> DeleteTag(int id)

        {

            var tag = await unitOfWork.TagRepository.GetTagByIdAsync(id);

            if (tag == null) return NotFound("Tag not found");



            await unitOfWork.TagRepository.DeleteTagAsync(tag);



            return NoContent();

        }

        [HttpGet("photo-approval-stats")]
        public async Task<ActionResult> GetPhotoApprovalCountAsync()
        {

            var stats = await unitOfWork.AdminRepository.GetPhotoApprovalCountAsync();

            return Ok(stats);

        }



        [HttpGet("users-without-main-photo")]
        public async Task<ActionResult> GetUsersWithoutMainPhoto()
        {
            var users = await unitOfWork.AdminRepository.GetUsersWithoutMainPhotoAsync();

            return Ok(users);

        }




    }
}
