using Microsoft.Extensions.Options;

using MovieService.Context;
using MovieService.Entity;
using MovieService.Repository;
using MovieService.Repository.Interface;
using MovieService.Repository.MongoDBRepo;
using MovieService.Service;
using MovieService.Service.Interface;
using MovieService.Events;
using MovieService.Messaging;
using MovieService.Messaging.Interface;
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
        builder.Services.AddScoped<IMovieService, Service.MovieService>();
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
        
        // đăng ký HttpClient
        // builder.Services.AddHttpClient().AddServiceDiscovery();

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

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
        
    }
}