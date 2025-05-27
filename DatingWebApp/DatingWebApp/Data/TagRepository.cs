using DatingWebApp.Entities;
using DatingWebApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingWebApp.Data
{
    public class TagRepository(Db_Context context) : ITagRepository
    {
        public async Task<Tag> GetOrCreateTagAsync(string tagName)
        {

            var tag = await context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);

            if (tag != null) return tag;

            tag = new Tag { Name = tagName };

            context.Tags.Add(tag);

            await context.SaveChangesAsync();

            return tag;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {

            return await context.Tags.ToListAsync();

        }
        public async Task<Tag?> GetTagByIdAsync(int id)
        {

            return await context.Tags.FindAsync(id);

        }
        public async Task AddTagAsync(Tag tag)
        {

            context.Tags.Add(tag);

            await context.SaveChangesAsync();

        }
        public async Task DeleteTagAsync(Tag tag)

        {

            context.Tags.Remove(tag);

            await context.SaveChangesAsync();
        }
    }
}
