namespace GatewayAPI.Middleware;

public class PagingMiddleware
{
    private readonly RequestDelegate _next;

    public PagingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // chỉ phân trang cho các GET request
        if (context.Request.Method == "GET")
        {
            Console.WriteLine("Đã vào middleware phân trang");
            // lấy params từ request
            var query = context.Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());
            
            //kiểm tra param page, tính cả trường hợp nếu page và limit không phải là số thì cũng sẽ set mặc định
            if (!query.ContainsKey("page") || !int.TryParse(query["page"], out var pageValue) || pageValue <= 0)
            {
                query["page"] = "1"; //giá trị mặc định
            }
            
            //kiểm tra param limit
            if (!query.ContainsKey("limit") || !int.TryParse(query["limit"], out var limitValue) || limitValue <= 0)
            {
                query["limit"] = "100"; //giá trị mặc định
            }
            
            //tạo lại query string mới với page và limit
            var modifiedQueryString = string.Join("&", query.Select(q => $"{q.Key}={q.Value}"));
            context.Request.QueryString = new QueryString($"?{modifiedQueryString}");
            
            //in thử query string mới
            Console.WriteLine($"Query string mới: {context.Request.QueryString}");
        }
        Console.WriteLine("Phân trang thành công");
        await _next(context); // chuyển tiếp request
        
    }
}