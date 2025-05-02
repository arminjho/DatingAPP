using AutoMapper;
using DatingWebApp.Data;
using DatingWebApp.DTOs;
using DatingWebApp.Entities;
using DatingWebApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingWebApp.Controllers
{
   
    public class UsersController(IUserRepository userRepository):BaseApiController
    {
         
        [HttpGet]
        public async Task< ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users=await userRepository.GetMembersAsync();
           
            return Ok(users);
        }
       
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string name)
        {
            var user = await userRepository.GetMemberAsync(name);
            if (user == null) return NotFound();
            return Ok(user);
        }

        

    }
}
