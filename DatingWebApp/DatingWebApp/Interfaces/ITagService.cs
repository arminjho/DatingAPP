using DatingWebApp.DTOs;

namespace DatingWebApp.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAllTagsAsync();
        Task<TagDto> AddTagAsync(TagDto tagDto);
        Task DeleteTagAsync(int id);
    }
}
