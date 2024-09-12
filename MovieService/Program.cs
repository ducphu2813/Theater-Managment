using Microsoft.Extensions.Options;

using MovieService.Context;
using MovieService.Entity;
using MovieService.Repository;
using MovieService.Repository.Interface;
using MovieService.Repository.MongoDBRepo;
using MovieService.Service.Interface;

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
        builder.Services.AddSingleton<MongoDBContext>();
        
        //đăng ký các Repository
        builder.Services.AddScoped(typeof(IRepository<>), typeof(MongoDBRepository<>));
        builder.Services.AddScoped<IMovieRepository, MovieRepository>();
        builder.Services.AddScoped<IMovieScheduleRepository, MovieScheduleRepository>();
        
        //đăng ký các Service
        builder.Services.AddScoped<IMovieService, Service.MovieService>();
        builder.Services.AddScoped<IMovieScheduleService, Service.MovieScheduleService>();
        
        //
        // ============================================
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