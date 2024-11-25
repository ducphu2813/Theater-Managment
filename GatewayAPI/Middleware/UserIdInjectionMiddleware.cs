using System.Security.Claims;
using System.Text;
using GatewayAPI.External;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        
        // bỏ qua middleware cho các endpoint không yêu cầu xác thực (ví dụ: /auth/api/login, /auth/api/register) 
        if (context.Request.Path.StartsWithSegments("/auth/api/login") 
            || context.Request.Path.StartsWithSegments("/auth/api/Mail")
            || context.Request.Path.StartsWithSegments("/auth/api/User")
            || context.Request.Path.StartsWithSegments("/auth/api/register")
            || ( (context.Request.Path.StartsWithSegments("/movies/api/Movie") 
                 || context.Request.Path.StartsWithSegments("/movies/api/Movie/getAll")
                 || context.Request.Path.StartsWithSegments("/movies/api/MovieSchedule")
                 || context.Request.Path.StartsWithSegments("/movies/api/MovieSchedule/getAll")
                 || context.Request.Path.StartsWithSegments("/movies/api/Room")
                 || context.Request.Path.StartsWithSegments("/movies/api/MovieSchedule/movie")
                 || context.Request.Path.StartsWithSegments("/movies/api/MovieSchedule/showDates"))
                 && context.Request.Method == "GET"))
        {
            await _next(context);
            return;
        }
        
        if (context.User.Identity.IsAuthenticated)
        {
            
            //gán user id vào query string nếu là endpoint /auth/api/verify
            if (context.Request.Path.StartsWithSegments("/auth/api/verify"))
            {
                // lấy params từ request
                var query = context.Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());
            
                var userId = context.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                Console.WriteLine("tìm thấy user id nè: "+userId);
            
                query["userId"] = userId;
                //tạo lại query string mới với user id
                context.Request.QueryString = QueryString.Create(query);
            
                //in thử query string mới
                Console.WriteLine($"Query string mới: {context.Request.QueryString}");
            }
            
            // kiểm tra nếu là POST request hoặc PUT request và có body
            if ((context.Request.Method == "POST" || context.Request.Method == "PUT") && context.Request.ContentType.Contains("application/json"))
            {
                // đọc request body
                context.Request.EnableBuffering();
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
                {
                    var requestBody = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0; // reset lại body stream

                    // giải mã body dưới dạng dynamic để xử lý linh hoạt
                    var jsonBody = JsonConvert.DeserializeObject<dynamic>(requestBody);

                    //kiểm tra body có phải là object hay array
                    if (jsonBody is JObject jsonObject)
                    {
                        // kiểm tra JSON body có trường "UserId"
                        if (jsonBody != null && jsonBody["UserId"] != null)
                        {
                            // Lấy user id từ claim trong JWT
                            var userId = context.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                            Console.WriteLine("tìm thấy user id nè: "+userId);
                            // Gán UserId từ JWT vào JSON body
                            jsonBody["UserId"] = userId;

                            // Serialize lại body đã được chỉnh sửa
                            var modifiedRequestBody = JsonConvert.SerializeObject(jsonBody);

                            // thay thế lại request body bằng body đã chỉnh sửa
                            var modifiedBodyStream = new MemoryStream(Encoding.UTF8.GetBytes(modifiedRequestBody));
                            context.Request.Body = modifiedBodyStream;
                        
                            // cập nhật Content-Length
                            context.Request.ContentLength = modifiedBodyStream.Length;
                        }   
                    }
                }
            }
            await _next(context); // chuyển tiếp request sau khi xử lý
        }
    }
}