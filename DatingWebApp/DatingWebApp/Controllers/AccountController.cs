using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using DatingWebApp.Data;
using DatingWebApp.DTOs;
using DatingWebApp.Entities;
using DatingWebApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingWebApp.Controllers
{
    public class AccountController(Db_Context context, ITokenService tokenService, IMapper mapper):BaseApiController
    {
        [HttpPost("register")]

        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExist(registerDto.Username))
                return BadRequest("Account with this username already exists, please log in");
            using var hmac = new HMACSHA512();
            var user = mapper.Map<AppUser>(registerDto);
            user.UserName = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt=hmac.Key;


            context.Users.Add(user);
            await context.SaveChangesAsync();
            return new UserDto
            {
                Username = user.UserName,
                Token = tokenService.CreateToken(user),
                KnownUs=user.KnownAs,
                Gender = user.Gender


            };
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>>Login(LoginDto loginDto)
        {
            var user=await context.Users.Include(p=>p.Photos)
                .FirstOrDefaultAsync(x=>x.UserName==loginDto.Username.ToLower());
            if (user==null) return Unauthorized("Invalid username");

            using var hmac=new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computedHash.Length; i++) {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }
            return new UserDto
            {  
                Username = user.UserName,
                KnownUs=user.KnownAs,
                Token = tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                Gender=user.Gender

            };
        }
        private async Task<bool> UserExist(string username) {
            return await context.Users.AnyAsync(u=>u.UserName.ToLower()==username.ToLower());
        
        }
    }
}
