using CloudinaryDotNet.Actions;
using DatingWebApp.DTOs;

namespace DatingWebApp.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
        Task<IEnumerable<PhotoWithTagsDto>> GetPhotosByTagsAsync(List<string> tags);
        Task<IEnumerable<PhotoWithTagsDto>> GetUnapprovedPhotosByTagsAsync(List<string> tags);
        Task ApprovePhotoAsync(int photoId);
        Task RejectPhotoAsync(int photoId);




    }
}
