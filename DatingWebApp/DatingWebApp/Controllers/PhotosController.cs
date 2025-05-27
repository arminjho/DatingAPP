using AutoMapper;
using DatingWebApp.DTOs;
using DatingWebApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingWebApp.Controllers
{
    [Authorize]
    public class PhotosController(IUnitOfWork unitOfWork, IMapper mapper) : BaseApiController
    {
        [HttpGet("filter-by-tags")]

        public async Task<ActionResult<IEnumerable<PhotoWithTagsDto>>> GetPhotosByTags([FromQuery] List<string> tags)

        {

            if (tags == null || !tags.Any())

                return BadRequest("At least one tag is required.");



            var photos = await unitOfWork.PhotoRepository.GetPhotosByTagsAsync(tags);



            return Ok(mapper.Map<IEnumerable<PhotoWithTagsDto>>(photos));

        }



        [Authorize(Policy = "ModeratePhotoRole")]

        [HttpGet("unapproved-by-tags")]

        public async Task<ActionResult<IEnumerable<PhotoWithTagsDto>>> GetUnapprovedPhotosByTags([FromQuery] List<string> tags)

        {

            if (tags == null || !tags.Any())

                return BadRequest("At least one tag is required.");



            var photos = await unitOfWork.PhotoRepository.GetUnapprovedPhotosByTagsAsync(tags);



            return Ok(mapper.Map<IEnumerable<PhotoWithTagsDto>>(photos));
        }
    }
}
