using Microsoft.Extensions.Options;
using UserService.Context;
using UserService.Entity;
using UserService.Repository;
using UserService.Repository.Interface;
using UserService.Repository.MongoDBRepo;
using UserService.Service;
using UserService.Service.Interface;

namespace UserService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        // ============================================
        // ============================================
        //đăng ký MongoDB
        //
        //lấy thông tin kết nối từ appsettings.json
        builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));
        builder.Services.AddSingleton<MongoDBSettings>(sp => sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);
        
        //đăng ký MongoDBContext
        builder.Services.AddScoped<MongoDBContext>();
        
        //đăng ký các Repository
        builder.Services.AddScoped(typeof(IRepository<>), typeof(MongoDBRepository<>));
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserDetailRepository, UserDetailRepository>();
        builder.Services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
        
        //đăng ký các Service
        builder.Services.AddScoped<IUserService, Service.UserService>();
        builder.Services.AddScoped<IUserDetailService, UserDetailService>();
        builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();
        
        //
        // ============================================

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}