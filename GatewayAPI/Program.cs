using System.IdentityModel.Tokens.Jwt;
using System.Text;
using GatewayAPI.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace GatewayAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        
        //disable csrf api
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        // thêm reverse proxy vào project
        builder.Services.AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
        
        //thêm các authorization policy tùy chỉnh
        //policy cho MANAGER và ADMIN
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("ManagerAdminPolicy", policy =>
                policy
                    .RequireAuthenticatedUser()
                    .RequireClaim(builder.Configuration["Jwt:RoleClaimType"], "MANAGER", "ADMIN"));
        });

        // builder.Services.AddAuthorization();
        //thêm phần xác thực jwt
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)), //thực hiện xác thực secret key
                    ValidIssuer = builder.Configuration["Jwt:Issuer"], //thực hiện xác thực Issuer
                    ValidAudience = builder.Configuration["Jwt:Audience"], //thực hiện xác thực Audience
                    ClockSkew = TimeSpan.Zero, //thời gian hết hạn của token
                    RoleClaimType = "roles"
                };
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // Disable claim type mapping
                o.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("vào on token validated");
                        // In ra các claim từ JWT sau khi xác thực thành công
                        var claims = context.Principal.Claims;
                        foreach (var claim in claims)
                        {
                            Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("vào on authentication failed");
                        // khi jwt hết hạn, 
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            Console.WriteLine("JWT Token expired");
                    
                            // Trả về lỗi 401 Unauthorized khi JWT hết hạn
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new {
                                message = "Token expired"
                            }));
                        }
                        
                        // ghi ra console
                        Console.WriteLine("JWT Authentication Failed");
                        Console.WriteLine(context.Exception.ToString());
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new {
                            message = "Invalid token"
                        }));
                    },
                    OnChallenge = context =>
                    {
                        Console.WriteLine("vào on challenge");
                        // khi ko có jwt 
                        if (context.AuthenticateFailure != null)
                        {
                            Console.WriteLine("No JWT token provided");
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                            {
                                message = "No token provided"
                            }));
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        
        //thêm cors ==> disable csrf
        app.UseCors("AllowAll");

        //thêm authentication
        app.UseAuthentication();
        
        //đăng ký middleware gắn userId vào request body
        app.UseMiddleware<UserIdInjectionMiddleware>();
        
        //đăng ký middleware phân trang
        app.UseMiddleware<PagingMiddleware>();
        
        app.UseAuthorization();

        app.MapControllers();
        
        app.MapReverseProxy();

        app.Run();
    }
}