
using Microsoft.Extensions.Options;
using ReservationService.Application.Messaging;
using ReservationService.Application.Messaging.Consumer;
using ReservationService.Application.Messaging.Publisher;
using ReservationService.Application.Service;
using ReservationService.Application.Service.Background;
using ReservationService.Core.Entity;
using ReservationService.Core.Events;
using ReservationService.Core.Interfaces.Messaging;
using ReservationService.Core.Interfaces.Repository;
using ReservationService.Core.Interfaces.Service;
using ReservationService.Infrastructure.Persistence.Context;
using ReservationService.Infrastructure.Persistence.Repository;
using ReservationService.Middleware;
using ReservationService.Repository;
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

//đăng ký các repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(MongoDBRepository<>));
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddScoped<IFoodRepository, FoodRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();

//đăng ký các service
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<IFoodService, FoodService>();
builder.Services.AddScoped<ISeatService, SeatService>();

//đăng ký RabbitMQ Settings
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQSettings"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<RabbitMQSettings>>().Value);

//đăng ký Consumer
// builder.Services.AddScoped<IConsumer<MovieScheduleEvent>, MovieScheduleConsumer<MovieScheduleEvent>>();
builder.Services.AddScoped<IConsumer<PaymentEvent>, PaymentConsumer<PaymentEvent>>();

//đăng ký Publisher
builder.Services.AddScoped(typeof(IPublisher<>), typeof(RabbitMQPublisher<>));
builder.Services.AddScoped<IPublisher<AdminEvent>, ProcessedTicketPublisher<AdminEvent>>();

//background service
// builder.Services.AddHostedService<ScheduleConsumerService>();
builder.Services.AddHostedService<PaymentConsumerService>();

//đăng ký Consul
builder.Services.AddServiceDiscovery(options => options.UseConsul());

//đăng ký các uri của các service khác
builder.Services.AddHttpClient("movie-service", client =>
{
    client.BaseAddress = new Uri("http://movie-service"); // Đây chỉ là base URL dựa trên ServiceName được đăng ký ở Consul
}).AddServiceDiscovery();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReservationService v1");
    });
}
//đăng ký middleware xử lý lỗi
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();