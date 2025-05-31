using DatingWebApp.DTOs;
using DatingWebApp.Interfaces;
using DatingWebApp.Middleware;
using DatingWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DatingWebApp.Controllers
{
    public class TagsController(ILogger<ExceptionMiddleware> logger, ITagService tagService) :BaseApiController
    {

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("tags")]

        public async Task<ActionResult<IEnumerable<TagDto>>> GetAllTags()
        {

            try
            {
                var tags = await tagService.GetAllTagsAsync();
                return Ok(tags);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving tags.");
                return StatusCode(500, "An unexpected error occurred while retrieving tags.");
            }


        }


        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("add-tag")]

        public async Task<ActionResult<TagDto>> AddTag([FromBody] TagDto tagDto)
        {

            try
            {
                var createdTag = await tagService.AddTagAsync(tagDto);
                return Ok(createdTag);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while adding a tag.");
                return StatusCode(500, "An unexpected error occurred while adding the tag.");
            }


        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("delete-tag/{id:int}")]
        public async Task<ActionResult> DeleteTag(int id)
        {

            try
            {
                await tagService.DeleteTagAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting the tag.");
                return StatusCode(500, "An unexpected error occurred while deleting the tag.");
            }

        }
    }
}
