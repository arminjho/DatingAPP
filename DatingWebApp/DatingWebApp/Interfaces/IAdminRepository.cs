using DatingWebApp.DTOs;

namespace DatingWebApp.Interfaces
{
    public interface IAdminRepository
    {
        Task<List<PhotoApprovalStatsDto>> GetPhotoApprovalCountAsync();
        Task<List<UserWithoutMainPhotoDto>> GetUsersWithoutMainPhotoAsync();
    }
}
