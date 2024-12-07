using MongoDB.Bson;

namespace Auth.API.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    
    public GlobalExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context); // tiếp tục pipeline
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex); // xử lý lỗi
        }
    }
    
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // cấu hình mã lỗi và thông báo dựa trên loại ngoại lệ
        int statusCode;
        string message;

        if (exception is InvalidOperationException)
        {
            statusCode = StatusCodes.Status400BadRequest; // lỗi 400 cho lỗi logic
            message = exception.Message; // trả về thông báo tùy chỉnh từ InvalidOperationException
        }
        else
        {
            statusCode = StatusCodes.Status500InternalServerError; // lỗi mặc định là 500
            message = "An unexpected error occurred.";
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        //lấy message từ exception
        //nếu message rỗng thì lấy message từ ngoại lệ
        message = string.IsNullOrEmpty(exception.Message) ? message : exception.Message;

        // trả về response json chứa thông tin lỗi
        return context.Response.WriteAsync(new
        {
            StatusCode = statusCode,
            Message = message
        }.ToJson()); // trả về json để client có thể dễ dàng xử lý
    }
}