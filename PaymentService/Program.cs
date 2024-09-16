using Microsoft.Extensions.Options;
using PaymentService.Context;
using PaymentService.Entity;
using PaymentService.Repository.MongoDBRepo;

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

//đăng ký các Service

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