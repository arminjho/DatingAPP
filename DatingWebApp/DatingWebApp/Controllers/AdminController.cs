using DatingWebApp.Entities;
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
        IPhotoService photoService, ILogger<ExceptionMiddleware> logger):BaseApiController
    {
        [Authorize(Policy ="RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users=await userManager.Users
                .OrderBy(x=>x.UserName)
                .Select(x=> new
                {
                    x.Id,
                    Username=x.UserName,
                    Roles=x.UserRoles.Select(x=>x.Role.Name).ToList()
                }).ToListAsync();
            return Ok(users);
        }
        [Authorize(Policy ="RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, string roles)
        {
            if (string.IsNullOrEmpty(roles)) { return BadRequest("you must select at least one role"); }
            var selectedRoles=roles.Split(',').ToArray();   
            
            var user= await userManager.FindByNameAsync(username);

            if (user == null) { return NotFound("User not found"); }

            var userRoles = await userManager.GetRolesAsync(user);

            var result = await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) { return BadRequest("Failed to add to roles"); }

            result=await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) {return BadRequest("Failed to remove from roles");}

            return Ok(await userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public async Task <ActionResult> GetPhotosForModeration()
        {
            var photos = await unitOfWork.PhotoRepository.GetUnapprovedPhotos();

            return Ok(photos);
        }


        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("approve-photo/{photoId}")]
        public async Task<ActionResult> ApprovePhoto(int photoId)
        {
            try 
            
           { var photo = await unitOfWork.PhotoRepository.GetPhotoById(photoId);

            if (photo == null) { return NotFound("Could not get photo from db"); }

            photo.IsApproved = true;

            var user = await unitOfWork.UserRepository.GetUserByPhotoId(photoId);

            if (user == null) { return NotFound("Could not get user from db"); }

            if (!user.Photos.Any(x => x.IsMain)) { photo.IsMain = true; }

            await unitOfWork.Complete();

                return Ok();
            }  
            
            catch (Exception ex)
            {
                logger.LogError(ex, "An error ocurred while approving the photo");
                return StatusCode(500);

            }
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("reject-photo/{photoId}")]
        public async Task<ActionResult> RejectPhoto(int photoId)
        {

            try

            {var photo = await unitOfWork.PhotoRepository.GetPhotoById(photoId);

            if (photo == null){ return NotFound("Could not get photo from db"); }

            if (photo.PublicId != null)
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicId);

                if (result.Result == "ok")
                {
                    unitOfWork.PhotoRepository.RemovePhoto(photo);
                }
            }
            else
            {
                unitOfWork.PhotoRepository.RemovePhoto(photo);
            }

            await unitOfWork.Complete();

                return Ok();
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "An error ocurred while rejecting the photo");
                return StatusCode(500);
            }
        }
    }
}
