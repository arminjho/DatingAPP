using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using DatingWebApp.Data;
using DatingWebApp.DTOs;
using DatingWebApp.Entities;
using DatingWebApp.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingWebApp.Controllers
{
    public class AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper):BaseApiController
    {
        [HttpPost("register")]

        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (registerDto.Username == null) throw new Exception("not valid data for register.dto.username");

            if (await UserExist(registerDto.Username))
                return BadRequest("Account with this username already exists, please log in");
            
            var user = mapper.Map<AppUser>(registerDto);
            user.UserName = registerDto.Username.ToLower();
         


             var result=await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return new UserDto
            {
                Username = user.UserName,
                Token = await tokenService.CreateToken(user),
                KnownUs=user.KnownAs,
                Gender = user.Gender


            };
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>>Login(LoginDto loginDto)
        {
            var user=await userManager.Users
                .Include(p=>p.Photos)
                .FirstOrDefaultAsync(x=>x.NormalizedUserName==loginDto.Username.ToUpper());

            if (user==null || user.UserName==null) return Unauthorized("Invalid username");

            var result=await userManager.CheckPasswordAsync(user,loginDto.Password);
            if(!result) return Unauthorized();
            return new UserDto
            {  
                Username = user.UserName,
                KnownUs=user.KnownAs,
                Token = await tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                Gender=user.Gender

            };
        }
        private async Task<bool> UserExist(string username) {
            return await userManager.Users.AnyAsync(u=>u.NormalizedUserName==username.ToUpper());
        
        }
    }
}
