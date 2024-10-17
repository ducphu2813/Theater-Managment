using System.Security.Claims;
using System.Text;
using GatewayAPI.External;
using Newtonsoft.Json;

namespace GatewayAPI.Middleware;

public class UserIdInjectionMiddleware
{
    private readonly RequestDelegate _next;

    public UserIdInjectionMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task Invoke(HttpContext context)
    {
        
        // Bỏ qua middleware cho các endpoint không yêu cầu xác thực (ví dụ: /auth/api/login)
        if (context.Request.Path.StartsWithSegments("/auth/api/login"))
        {
            await _next(context);
            return;
        }
        
        if (context.User.Identity.IsAuthenticated)
        {
            // Kiểm tra nếu là POST request và có body
            if (context.Request.Method == "POST" && context.Request.ContentType.Contains("application/json"))
            {
                // Đọc request body
                context.Request.EnableBuffering();
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
                {
                    var requestBody = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0; // Reset lại body stream

                    // Giải mã body dưới dạng dynamic để xử lý linh hoạt
                    var jsonBody = JsonConvert.DeserializeObject<dynamic>(requestBody);

                    // Kiểm tra nếu JSON body có trường "UserId"
                    if (jsonBody != null && jsonBody["UserId"] != null)
                    {
                        // Lấy user id từ claim trong JWT
                        var userId = context.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                        Console.WriteLine("tìm thấy user id nè: "+userId);
                        // Gán UserId từ JWT vào JSON body
                        jsonBody["UserId"] = userId;

                        // Serialize lại body đã được chỉnh sửa
                        var modifiedRequestBody = JsonConvert.SerializeObject(jsonBody);

                        // Thay thế lại request body bằng body đã chỉnh sửa
                        var modifiedBodyStream = new MemoryStream(Encoding.UTF8.GetBytes(modifiedRequestBody));
                        context.Request.Body = modifiedBodyStream;
                        
                        // Cập nhật Content-Length
                        context.Request.ContentLength = modifiedBodyStream.Length;
                    }
                }
            }
            await _next(context); // Chuyển tiếp request sau khi xử lý
        }
    }
}