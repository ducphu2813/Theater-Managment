using MongoDB.Bson;

namespace ReservationService.Middleware;

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
        // Cấu hình mã lỗi và thông báo dựa trên loại ngoại lệ
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
            message = exception.Message; // trả về thông báo lỗi mặc định
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        // Trả về response JSON chứa thông tin lỗi
        return context.Response.WriteAsync(new
        {
            StatusCode = statusCode,
            Message = message
        }.ToJson());
    }
}