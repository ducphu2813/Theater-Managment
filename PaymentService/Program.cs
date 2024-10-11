using Microsoft.Extensions.Options;
using PaymentService.Context;
using PaymentService.Entity;
using PaymentService.Events;
using PaymentService.Messaging;
using PaymentService.Messaging.Interface;
using PaymentService.Messaging.Publisher;
using PaymentService.Repository;
using PaymentService.Repository.Interface;
using PaymentService.Repository.MongoDBRepo;
using PaymentService.Service.Interface;

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

//đăng ký RabbitMQ Settings
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQSettings"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<RabbitMQSettings>>().Value);

//đăng ký các publisher
builder.Services.AddSingleton<IPublisher<PaymentEvent>, PaymentPublisher<PaymentEvent>>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();