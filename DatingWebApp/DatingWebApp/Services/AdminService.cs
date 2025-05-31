
using System.Text.RegularExpressions;
using AutoMapper;
using DatingWebApp.Data;
using DatingWebApp.DTOs;
using DatingWebApp.Entities;
using DatingWebApp.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingWebApp.Services
{
    public class AdminService:IAdminService
    {
       
            private readonly UserManager<AppUser> _userManager;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;


        public AdminService(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IMapper mapper)
            {
                _userManager = userManager;
                _unitOfWork=unitOfWork;
                _mapper = mapper;
            }

            public async Task<List<UserWithRolesDto>> GetUsersWithRolesAsync()
            {
                var users = await _userManager.Users
                    .OrderBy(x => x.UserName)
                    .Select(x => new UserWithRolesDto
                    {
                        Id = x.Id,
                        Username = x.UserName,
                        Roles = x.UserRoles.Select(r => r.Role.Name).ToList()
                    }).ToListAsync();

                return users;
            }


        public async Task<IList<string>> EditUserRolesAsync(string username, string[] roles)
        {
            if (roles == null || roles.Length == 0)
                throw new ArgumentException("At least one role must be selected.");

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var userRoles = await _userManager.GetRolesAsync(user);

            var addResult = await _userManager.AddToRolesAsync(user, roles.Except(userRoles));
            if (!addResult.Succeeded)
                throw new InvalidOperationException("Failed to add roles.");

            var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(roles));
            if (!removeResult.Succeeded)
                throw new InvalidOperationException("Failed to remove roles.");

            return await _userManager.GetRolesAsync(user);
        }

     

        public async Task<object> GetPhotoApprovalStatisticsByUser()
        {
            var stats = await _unitOfWork.AdminRepository.GetPhotoApprovalStatisticsByUser();
            return stats;
        }

        public async Task<IEnumerable<UserWithoutMainPhotoDto>> GetUsersWithoutMainPhotoAsync()
        {
            var users = await _unitOfWork.AdminRepository.GetUsersWithoutMainPhotoAsync();
            return _mapper.Map<IEnumerable<UserWithoutMainPhotoDto>>(users);
        }



    }
}
