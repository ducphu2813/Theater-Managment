using Auth.API.Middleware;
using Auth.Application.Interfaces;
using Auth.Application.Service;
using Auth.Domain.Interfaces;
using Auth.Domain.Settings;
using Auth.Infrastructure.Repository;
using Auth.Infrastructure.Token;
using Microsoft.Extensions.Options;
using Shared.Context;
using Shared.Interfaces;
using Shared.Repository;
using Shared.Settings;

namespace Auth.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        //đăng ký token provider
        builder.Services.AddSingleton<TokenProvider>();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        // đăng ký MongoDB
        //lấy thông tin kết nối từ appsettings.json
        builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDbSettings"));
        builder.Services.AddSingleton<MongoDBSettings>(sp => sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);
        
        //đăng ký MongoDBContext
        builder.Services.AddScoped<MongoDBContext>();
        
        //đăng ký EmailSettings từ appsettings.json
        builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
        
        //đăng ký các Repository
        builder.Services.AddScoped(typeof(IRepository<>), typeof(MongoDBRepository<>));
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        builder.Services.AddScoped<IResetPasswordRepository, ResetPasswordRepository>();
        
        //đăng ký các Service
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IRoleService, RoleService>();
        builder.Services.AddScoped<IEmailService, EmailService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        //đăng ký middleware xử lý lỗi
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}