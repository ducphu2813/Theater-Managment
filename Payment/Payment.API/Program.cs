using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using Payment.API.Middleware;
using Payment.Application.Interfaces;
using Payment.Application.Messaging.Publisher;
using Payment.Application.Service;
using Payment.Domain.Events;
using Payment.Domain.Interfaces;
using Payment.Domain.Settings;
using Payment.Infrastructure.Repository;
using Shared.Context;
using Shared.Interfaces;
using Shared.Interfaces.Messaging;
using Shared.Repository;
using Shared.Settings;
using Steeltoe.Common.Http.Discovery;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Consul;

namespace Payment.API;

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
        builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
        
        //đăng ký các uri của các service khác
        builder.Services.AddHttpClient("reservation-service", client =>
        {
            client.BaseAddress = new Uri("http://reservation-service"); // đây chỉ là base URL dựa trên ServiceName được đăng ký ở Consul
        }).AddServiceDiscovery();
        
        //đăng ký các Service
        builder.Services.AddScoped<IPaymentService, PaymentService>();
        builder.Services.AddScoped<IVnPayService, VnPayService>();
        
        //đăng ký RabbitMQ Settings
        builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQSettings"));
        builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<RabbitMQSettings>>().Value);
        
        //đăng ký các publisher
        builder.Services.AddSingleton<IPublisher<PaymentEvent>, PaymentPublisher<PaymentEvent>>();
        
        // đăng ký IActionContextAccessor
        builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        
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