using System.Text;
using DatingWebApp.Data;
using DatingWebApp.Entities;
using DatingWebApp.Extensions;
using DatingWebApp.Interfaces;
using DatingWebApp.Middleware;
using DatingWebApp.Services;
using DatingWebApp.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddIdentityService(builder.Configuration);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost4200",
        builder => builder.WithOrigins("http://localhost:4200")
                          .AllowCredentials()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter JWT with Bearer into field",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                 new OpenApiSecurityScheme
               {
                  Reference = new OpenApiReference
             {
               Type = ReferenceType.SecurityScheme,
               Id = "Bearer"
             }
               },
                           new string[] { }
                }
            });
        });


var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();



//app.UseCors(x=>x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "https://localhost:4200"));
app.UseCors("AllowLocalhost4200");

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1"));
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try 
{
    var context = services.GetRequiredService<Db_Context>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager=services.GetRequiredService<RoleManager<AppRole>>();    
    await context.Database.MigrateAsync();
    await context.Database.ExecuteSqlRawAsync("DELETE FROM `Connections`");
    await Seed.SeedUsers(userManager, roleManager);
}
catch (Exception ex) 
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();
