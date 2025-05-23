using DatingWebApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatingWebApp.Data
{
    public class Db_Context(DbContextOptions options):IdentityDbContext<AppUser,AppRole,int, 
        IdentityUserClaim<int>,AppUserRole, IdentityUserLogin<int>,IdentityRoleClaim<int>,
        IdentityUserToken<int>> (options)
    {

        public DbSet<UserLike> Likes { get; set; }
        public DbSet <Message> Messages { get; set; }
        public DbSet <Group> Groups { get; set; }
        public DbSet <Connection> Connections { get; set; }
        public DbSet <Photo> Photos { get; set; }
        public DbSet <Tag> Tags { get; set; }
        public DbSet <PhotoTag> PhotoTags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            builder.Entity<UserLike>()
                .HasKey(k => new { k.SourceUserId, k.TargetUserId });

            builder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserLike>()
                .HasOne(s => s.TargetUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(s => s.TargetUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Message>()
                .HasOne(x => x.Recipient)
                .WithMany(x => x.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(x => x.Sender)
                .WithMany(x => x.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Photo>().HasQueryFilter(p => p.IsApproved);

            //task2

            builder.Entity<PhotoTag>()

    .HasKey(pt => new { pt.PhotoId, pt.TagId });



            builder.Entity<PhotoTag>()

                .HasOne(pt => pt.Photo)

                .WithMany(p => p.PhotoTags)

                .HasForeignKey(pt => pt.PhotoId);



            builder.Entity<PhotoTag>()

                .HasOne(pt => pt.Tag)

                .WithMany(t => t.PhotoTags)

                .HasForeignKey(pt => pt.TagId);


        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=127.0.0.1;Database=dating_db1;User=root;Password=2611;",
                new MySqlServerVersion(new Version(10, 5, 4)));
        }



    }
}
