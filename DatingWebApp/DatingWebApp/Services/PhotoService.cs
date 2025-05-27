using System.Net;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingWebApp.Data;
using DatingWebApp.DTOs;
using DatingWebApp.Helpers;
using DatingWebApp.Interfaces;
using Microsoft.Extensions.Options;

namespace DatingWebApp.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PhotoService(IOptions<CloudinarySettings> config, IUnitOfWork unitOfWork, IMapper mapper)
        {
            var acc = new Account
            (
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }


        public async Task<IEnumerable<PhotoWithTagsDto>> GetPhotosByTagsAsync(List<string> tags)
        {
            if (tags == null || !tags.Any())
                { throw new ArgumentException("At least one tag is required."); }

            var photos = await _unitOfWork.PhotoRepository.GetPhotosByTagsAsync(tags);
            return _mapper.Map<IEnumerable<PhotoWithTagsDto>>(photos);
        }

        public async Task<IEnumerable<PhotoWithTagsDto>> GetUnapprovedPhotosByTagsAsync(List<string> tags)
        {
            if (tags == null || !tags.Any())
                throw new ArgumentException("At least one tag is required.");

            var photos = await _unitOfWork.PhotoRepository.GetUnapprovedPhotosByTagsAsync(tags);
            return _mapper.Map<IEnumerable<PhotoWithTagsDto>>(photos);
        }

        public async Task ApprovePhotoAsync(int photoId)
        {
            var photo = await _unitOfWork.PhotoRepository.GetPhotoById(photoId);
            if (photo == null)
                throw new KeyNotFoundException("Could not get photo from db");

            photo.IsApproved = true;

            var user = await _unitOfWork.UserRepository.GetUserByPhotoId(photoId);
            if (user == null)
                throw new KeyNotFoundException("Could not get user from db");

            if (!user.Photos.Any(x => x.IsMain))
                photo.IsMain = true;

            var success = await _unitOfWork.Complete();
            if (!success)
                throw new Exception("Failed to approve photo");
        }

        public async Task RejectPhotoAsync(int photoId)
        {
            var photo = await _unitOfWork.PhotoRepository.GetPhotoById(photoId);
            if (photo == null)
                throw new KeyNotFoundException("Could not get photo from db");

            if (!string.IsNullOrEmpty(photo.PublicId))
            {
                var result = await DeletePhotoAsync(photo.PublicId); 
                if (result.Result == "ok")
                {
                    _unitOfWork.PhotoRepository.RemovePhoto(photo);
                }
                else
                {
                    throw new InvalidOperationException("Failed to delete photo from external service");
                }
            }
            else
            {
                _unitOfWork.PhotoRepository.RemovePhoto(photo);
            }

            var success = await _unitOfWork.Complete();
            if (!success)
                throw new Exception("Problem rejecting photo");
        }



        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result;
        }
    }
}
