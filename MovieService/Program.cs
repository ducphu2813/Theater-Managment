
using Microsoft.Extensions.Options;
using MovieService.API.Middleware;
using MovieService.Application.Messaging;
using MovieService.Application.Service;
using MovieService.Core.Entity;
using MovieService.Core.Interfaces.Repository;
using MovieService.Core.Interfaces.Service;
using MovieService.Infrastructure.Persistence.Context;
using MovieService.Infrastructure.Persistence.Repositories;
using Steeltoe.Common.Http.Discovery;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Consul;

namespace MovieService;

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
        
        //đăng ký các Service
        builder.Services.AddScoped<IMovieService, Application.Service.MovieService>();
        builder.Services.AddScoped<IMovieScheduleService, MovieScheduleService>();
        builder.Services.AddScoped<IRoomService, RoomService>();
        
        
        //đăng ký rabbitMQ settings
        builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQSettings"));
        builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<RabbitMQSettings>>().Value);
        
        //đăng ký các consumer và publisher
        // builder.Services.AddSingleton<IPublisher<MovieScheduleEvent>, RabbitMQPublisher<MovieScheduleEvent>>();
        // builder.Services.AddSingleton<IConsumer<String>, RabbitMQConsumer<String>>();
        
        
        //
        // ============================================
        // ============================================
        
        //đăng ký Consul
        builder.Services.AddServiceDiscovery(options  => options .UseConsul());
        
        //đăng ký reservation service
        builder.Services.AddHttpClient("reservation-service", client =>
        {
            client.BaseAddress = new Uri("http://reservation-service"); // Đây chỉ là base URL dựa trên ServiceName được đăng ký ở Consul
        }).AddServiceDiscovery();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MovieService v1");
            });
        }
        
        //đăng ký middleware xử lý lỗi
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

        app.UseHttpsRedirection();

        // app.UseAuthentication(); // phần xác thực
        
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
        
    }
}