using Microsoft.Extensions.Options;
using Reservation.API.Middleware;
using Reservation.Application.Interfaces.Service;
using Reservation.Application.Messaging.Consumer;
using Reservation.Application.Messaging.Publisher;
using Reservation.Application.Service;
using Reservation.Application.Service.Background;
using Reservation.Domain.Events;
using Reservation.Domain.Interfaces;
using Reservation.Domain.Settings;
using Reservation.Infrastructure.Repository;
using Shared.Context;
using Shared.Interfaces;
using Shared.Interfaces.Messaging;
using Shared.Repository;
using Shared.Settings;
using Steeltoe.Common.Http.Discovery;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Consul;

namespace Reservation.API;

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
        
        //đăng ký các repository
        builder.Services.AddScoped(typeof(IRepository<>), typeof(MongoDBRepository<>));
        builder.Services.AddScoped<ITicketRepository, TicketRepository>();
        builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
        builder.Services.AddScoped<IFoodRepository, FoodRepository>();
        builder.Services.AddScoped<ISeatRepository, SeatRepository>();
        
        //đăng ký các uri của các service khác
        builder.Services.AddHttpClient("movie-service", client =>
        {
            client.BaseAddress = new Uri("http://movie-service"); // Đây chỉ là base URL dựa trên ServiceName được đăng ký ở Consul
        }).AddServiceDiscovery();
        
        //đăng ký các service
        builder.Services.AddScoped<ITicketService, TicketService>();
        builder.Services.AddScoped<IDiscountService, DiscountService>();
        builder.Services.AddScoped<IFoodService, FoodService>();
        builder.Services.AddScoped<ISeatService, SeatService>();
        
        //đăng ký RabbitMQ Settings
        builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQSettings"));
        builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<RabbitMQSettings>>().Value);
        
        //đăng ký consumer
        builder.Services.AddScoped<IConsumer<PaymentEvent>, PaymentConsumer<PaymentEvent>>();
        
        //đăng ký Publisher
        builder.Services.AddScoped<IPublisher<AdminEvent>, ProcessedTicketPublisher<AdminEvent>>();

        //background service
        builder.Services.AddHostedService<PaymentConsumerService>();
        
        //đăng ký Consul
        builder.Services.AddServiceDiscovery(options => options.UseConsul());
        
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