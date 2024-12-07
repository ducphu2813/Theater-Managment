using Microsoft.Extensions.Options;
using Movie.API.Middleware;
using Movie.Application.Interfaces;
using Movie.Application.Service;
using Movie.Domain.Interface;
using Movie.Infrastructure.Repository;
using Shared.Context;
using Shared.Interfaces;
using Shared.Repository;
using Shared.Settings;
using Steeltoe.Common.Http.Discovery;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Consul;

namespace Movie.API;

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
        builder.Services.AddScoped<IMovieRepository, MovieRepository>();
        builder.Services.AddScoped<IMovieScheduleRepository, MovieScheduleRepository>();
        builder.Services.AddScoped<IRoomRepository, RoomRepository>();
        
        //đăng ký reservation service http client
        builder.Services.AddHttpClient("reservation-service", client =>
        {
            client.BaseAddress = new Uri("http://reservation-service"); // Đây chỉ là base URL dựa trên ServiceName được đăng ký ở Consul
        }).AddServiceDiscovery();
        
        //đăng ký các Service
        builder.Services.AddScoped<IMovieService, MovieService>();
        builder.Services.AddScoped<IMovieScheduleService, MovieScheduleService>();
        builder.Services.AddScoped<IRoomService, RoomService>();
        
        //đăng ký Consul
        builder.Services.AddServiceDiscovery(options  => options .UseConsul());

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