using DatingWebApp.Entities;

namespace DatingWebApp.Interfaces
{
    public interface ITagRepository
    {
        Task<Tag> GetOrCreateTagAsync(string tagName);
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<Tag?> GetTagByIdAsync(int id);
        Task AddTagAsync(Tag tag);
        Task DeleteTagAsync(Tag tag);
    }
}
