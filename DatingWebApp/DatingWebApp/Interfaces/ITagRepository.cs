using DatingWebApp.Entities;

namespace DatingWebApp.Interfaces
{
    public interface ITagRepository
    {
        Task<Tag> GetOrCreateTagAsync(string tagName);
    }
}
