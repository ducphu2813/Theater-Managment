using System.Text;
using AuthService.Context;
using AuthService.Entity;
using AuthService.Repository;
using AuthService.Repository.Interface;
using AuthService.Repository.MongoDBRepo;
using AuthService.Service;
using AuthService.Service.Interface;
using AuthService.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthService;

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
        
        //đăng ký các Repository
        builder.Services.AddScoped(typeof(IRepository<>), typeof(MongoDBRepository<>));
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        
        //đăng ký các Service
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IRoleService, RoleService>();
        
        //thêm phần xác thực jwt
        // builder.Services.AddAuthorization();
        // builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //     .AddJwtBearer(o =>
        //     {
        //         o.RequireHttpsMetadata = false;
        //         o.TokenValidationParameters = new TokenValidationParameters
        //         {
        //             IssuerSigningKey =
        //                 new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)), //thực hiện xác thực secret key
        //             ValidIssuer = builder.Configuration["Jwt:Issuer"], //thực hiện xác thực Issuer
        //             ValidAudience = builder.Configuration["Jwt:Audience"], //thực hiện xác thực Audience
        //             ClockSkew = TimeSpan.Zero //xác thực thời gian hết hạn của token
        //         };
        //     });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        
        //thêm xác thực
        // app.UseAuthentication();

        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}