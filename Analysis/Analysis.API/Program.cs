using Analysis.API.Middleware;
using Analysis.Application.Interfaces;
using Analysis.Application.Messaging;
using Analysis.Application.Service;
using Analysis.Application.Service.Background;
using Analysis.Domain.Event;
using Analysis.Domain.Interfaces;
using Analysis.Domain.Settings;
using Analysis.Infrastructure.Repository;
using Microsoft.Extensions.Options;
using Shared.Context;
using Shared.Interfaces;
using Shared.Interfaces.Messaging;
using Shared.Repository;
using Shared.Settings;

namespace Analysis.API;

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
        
        // đăng ký MongoDB
        //lấy thông tin kết nối từ appsettings.json
        builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDbSettings"));
        builder.Services.AddSingleton<MongoDBSettings>(sp => sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);
        
        //đăng ký MongoDBContext
        builder.Services.AddScoped<MongoDBContext>();
        
        //đăng ký các Repository
        builder.Services.AddScoped(typeof(IRepository<>), typeof(MongoDBRepository<>));
        builder.Services.AddScoped<IMovieSaleRepository, MovieSaleRepository>();
        
        //đăng ký các Service
        builder.Services.AddScoped<IMovieSaleService, MovieSaleService>();
        
        //đăng ký rabbitMQ settings
        builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQSettings"));
        builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<RabbitMQSettings>>().Value);
        
        //đăng ký consumer
        builder.Services.AddScoped<IConsumer<TicketProcessedConsume>, TicketConsumer<TicketProcessedConsume>>();
        
        //đăng ký background service
        builder.Services.AddHostedService<TicketConsumService>();

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