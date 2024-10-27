using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using PaymentService.Context;
using PaymentService.Entity;
using PaymentService.Events;
using PaymentService.Messaging;
using PaymentService.Messaging.Interface;
using PaymentService.Messaging.Publisher;
using PaymentService.Middleware;
using PaymentService.Repository;
using PaymentService.Repository.Interface;
using PaymentService.Repository.MongoDBRepo;
using PaymentService.Service;
using PaymentService.Service.Interface;
using Steeltoe.Common.Http.Discovery;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Consul;

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

//đăng ký các Service
builder.Services.AddScoped<IPaymentService, PaymentService.Service.PaymentService>();
builder.Services.AddScoped<IVnPayService, VnPayService>();

//đăng ký RabbitMQ Settings
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQSettings"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<RabbitMQSettings>>().Value);

//đăng ký các publisher
builder.Services.AddSingleton<IPublisher<PaymentEvent>, PaymentPublisher<PaymentEvent>>();

// đăng ký IUrlHelperFactory and IActionContextAccessor
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();

//đăng ký Consul
builder.Services.AddServiceDiscovery(options => options.UseConsul());

//đăng ký các uri của các service khác
builder.Services.AddHttpClient("reservation-service", client =>
{
    client.BaseAddress = new Uri("http://reservation-service"); // đây chỉ là base URL dựa trên ServiceName được đăng ký ở Consul
}).AddServiceDiscovery();

var app = builder.Build();

// Configure the HTTP request pipeline.
//cấu hình swagger cho docker container
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PaymentService v1");
    });
}
//đăng ký middleware xử lý lỗi
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();