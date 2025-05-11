using DatingWebApp.Data;
using DatingWebApp.Helpers;
using DatingWebApp.Interfaces;
using DatingWebApp.Services;
using DatingWebApp.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DatingWebApp.Extensions
{
    public static class ApplicationServiceExtensions
    {

        public static IServiceCollection AddApplicationService
            (this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();
            services.AddDbContext<Db_Context>(opt =>
            {
                opt.UseMySql(config.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(10, 5, 4)));
            });

            
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<ILikesRepository, LikesRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();    
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<LogUserActivity>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddSignalR();
            services.AddSingleton<PresenceTracker>();
            services.AddCors();
            return services;
        }
    }
}
