using AutoMapper;
using DatingWebApp.DTOs;
using DatingWebApp.Interfaces;
using DatingWebApp.Middleware;
using DatingWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DatingWebApp.Controllers
{
    [Authorize]
    public class PhotosController(IUnitOfWork unitOfWork, ILogger<ExceptionMiddleware> logger, IPhotoService photoService) : BaseApiController
    {
        [HttpGet("filter-by-tags")]
        public async Task<ActionResult<IEnumerable<PhotoWithTagsDto>>> GetPhotosByTags([FromQuery] List<string> tags)
        {

            try
            {
                var photos = await photoService.GetPhotosByTagsAsync(tags);
                return Ok(photos);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "An error ocurred while filtering the photos by tags");
                return StatusCode(500);
            }

        }



        [Authorize(Policy = "ModeratePhotoRole")]

        [HttpGet("unapproved-by-tags")]

        public async Task<ActionResult<IEnumerable<PhotoWithTagsDto>>> GetUnapprovedPhotosByTags([FromQuery] List<string> tags)
        {

            try
            {
                var photos = await photoService.GetUnapprovedPhotosByTagsAsync(tags);
                return Ok(photos);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "An error ocurred while filtering the unapproved photos");
                return StatusCode(500);
            }

        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public async Task<ActionResult> GetPhotosForModeration()
        {
            var photos = await unitOfWork.PhotoRepository.GetUnapprovedPhotos();

            return Ok(photos);
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("approve-photo/{photoId}")]
        public async Task<ActionResult> ApprovePhoto(int photoId)
        {

            try
            {
                await photoService.ApprovePhotoAsync(photoId);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while approving the photo");
                return StatusCode(500, "Internal server error");
            }

        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("reject-photo/{photoId}")]
        public async Task<ActionResult> RejectPhoto(int photoId)
        {


            try
            {
                await photoService.RejectPhotoAsync(photoId);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while rejecting the photo");
                return StatusCode(500, "Internal server error");
            }

        }
    }
}
