
using Microsoft.Extensions.Options;
using ReservationService.Context;
using ReservationService.Entity;
using ReservationService.Events;
using ReservationService.Messaging;
using ReservationService.Messaging.Interface;
using ReservationService.Repository;
using ReservationService.Repository.Interface;
using ReservationService.Repository.MongoDBRepo;
using ReservationService.Service;
using ReservationService.Service.Background;
using ReservationService.Service.Interface;

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

//đăng ký RabbitMQConsumer và RabbitMQPublisher
builder.Services.AddScoped<IConsumer<MovieScheduleEvent>, RabbitMQConsumer<MovieScheduleEvent>>();
builder.Services.AddScoped(typeof(IPublisher<>), typeof(RabbitMQPublisher<>));

//background service
builder.Services.AddHostedService<MovieScheduleConsumer>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();