﻿using DatingWebApp.DTOs;
using DatingWebApp.Entities;
using DatingWebApp.Extensions;
using DatingWebApp.Helpers;
using DatingWebApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingWebApp.Controllers
{
    [Authorize]
    public class LikesController(IUnitOfWork unitOfWork): BaseApiController
    {
        [HttpPost("{targetUserId:int}")]
        public async Task<ActionResult> ToggleLike(int targetUserId)
        {
            var sourceUserId = User.GetUserId();
            if (sourceUserId == targetUserId) return BadRequest("You cannot like yourself");
            var existingLike=await unitOfWork.LikesRepository.GetUserLike(sourceUserId,targetUserId);
            if (existingLike == null) {

                var like = new UserLike
                {
                    SourceUserId = sourceUserId,
                    TargetUserId = targetUserId
                };
                unitOfWork.LikesRepository.Addlike(like);

            }
            else
            {
                unitOfWork.LikesRepository.DeleteLike(existingLike);
            }
            if (await unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to update like");
        }
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIds()
        {
            return Ok(await unitOfWork.LikesRepository.GetCurrentUserLikeIds(User.GetUserId()));   
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLikes([FromQuery] LikesParams likesParams)
        {
            likesParams.UserId=User.GetUserId();
            var users=await unitOfWork.LikesRepository.GetUserLikes(likesParams);
            Response.AddPaginationHeader(users);
            return Ok(users);
        }
    }
}
