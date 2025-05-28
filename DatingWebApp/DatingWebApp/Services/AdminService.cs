
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

        public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
        {
            var tags = await _unitOfWork.TagRepository.GetAllTagsAsync();
            return _mapper.Map<IEnumerable<TagDto>>(tags);
        }


        public async Task<TagDto> AddTagAsync(TagDto tagDto)
        {
            if (string.IsNullOrWhiteSpace(tagDto.Name))
                throw new ArgumentException("Tag name is required.");

            var tagName = tagDto.Name.Trim();

            if (!Regex.IsMatch(tagName, @"^[a-zA-Z0-9\s\-]{2,30}$"))
                throw new ArgumentException("Tag name contains invalid characters or is too short/long.");

            var existingTags = await _unitOfWork.TagRepository.GetAllTagsAsync();

            if (existingTags.Any(t => t.Name.Equals(tagName, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("A tag with the same name already exists.");

            var tag = new Tag { Name = tagName };

            await _unitOfWork.TagRepository.AddTagAsync(tag);

            return _mapper.Map<TagDto>(tag);
        }

        public async Task DeleteTagAsync(int id)
        {
            var tag = await _unitOfWork.TagRepository.GetTagByIdAsync(id);

            if (tag == null)
                throw new KeyNotFoundException("Tag not found.");

            await _unitOfWork.TagRepository.DeleteTagAsync(tag);
        }

        public async Task<object> GetPhotoApprovalStatsAsync()
        {
            var stats = await _unitOfWork.AdminRepository.GetPhotoApprovalCountAsync();
            return stats;
        }

        public async Task<IEnumerable<UserWithoutMainPhotoDto>> GetUsersWithoutMainPhotoAsync()
        {
            var users = await _unitOfWork.AdminRepository.GetUsersWithoutMainPhotoAsync();
            return _mapper.Map<IEnumerable<UserWithoutMainPhotoDto>>(users);
        }



    }
}
