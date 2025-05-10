using DatingWebApp.DTOs;
using DatingWebApp.Entities;
using DatingWebApp.Helpers;

namespace DatingWebApp.Interfaces
{
    
        public interface ILikesRepository
        {
            Task<UserLike?> GetUserLike(int sourceUserId, int targetUserId);
            Task<PagedList<MemberDto>> GetUserLikes(LikesParams likesParams);
            Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId);
            void DeleteLike(UserLike like);
            void Addlike(UserLike like);
       
        }
    
}
