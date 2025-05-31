using System.Text.RegularExpressions;
using AutoMapper;
using DatingWebApp.DTOs;
using DatingWebApp.Entities;
using DatingWebApp.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DatingWebApp.Services
{
    public class TagService:ITagService
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TagService(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
    }
}
