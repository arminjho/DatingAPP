using DatingWebApp.DTOs;
using DatingWebApp.Entities;

namespace DatingWebApp.Interfaces
{
    public interface IPhotoRepository
    {
        Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotos();
        Task<IEnumerable<Photo>> GetPhotosByTagsAsync(List<string> tagNames);
        Task<IEnumerable<Photo>> GetUnapprovedPhotosByTagsAsync(List<string> tagNames);
        Task<Photo?> GetPhotoById(int id);
        void RemovePhoto(Photo photo);

    }
}
