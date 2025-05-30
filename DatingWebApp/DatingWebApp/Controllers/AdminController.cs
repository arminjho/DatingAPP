﻿using System.Text.RegularExpressions;
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
using static DatingWebApp.Interfaces.IAdminService;

namespace DatingWebApp.Controllers
{
    public class AdminController(ILogger<ExceptionMiddleware> logger, IAdminService adminService) : BaseApiController
    {
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {

            try
            {
                var users = await adminService.GetUsersWithRolesAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving users");
                return StatusCode(500, "An error occurred while retrieving users");
            }

        }


        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromBody] string[] roles)
        {

            try
            {
                var updatedRoles = await adminService.EditUserRolesAsync(username, roles);
                return Ok(updatedRoles);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
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
                logger.LogError(ex, "An error occurred while editing user roles.");
                return StatusCode(500, "An unexpected error occurred while editing roles.");
            }

        }



        [HttpGet("photo-approval-stats")]
        public async Task<ActionResult> GetPhotoApprovalStatisticsByUser()
        {


            try
            {
                var stats = await adminService.GetPhotoApprovalStatisticsByUser();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving photo approval stats.");
                return StatusCode(500, "An unexpected error occurred while retrieving photo approval statistics.");
            }


        }



        [HttpGet("users-without-main-photo")]
        public async Task<ActionResult> GetUsersWithoutMainPhoto()
        {

            try
            {
                var users = await adminService.GetUsersWithoutMainPhotoAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving users without main photo.");
                return StatusCode(500, "An unexpected error occurred while retrieving users.");
            }


        }




    }
}
