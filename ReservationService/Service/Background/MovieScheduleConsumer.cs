

using ReservationService.Events;
using ReservationService.Messaging.Interface;

namespace ReservationService.Service.Background;

public class MovieScheduleConsumer : BackgroundService
{
    private readonly IConsumer<MovieScheduleEvent> _consumer;
    
    public MovieScheduleConsumer(IConsumer<MovieScheduleEvent> consumer)
    {
        _consumer = consumer;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _consumer.Consume(async (message) =>
            {
                // In message ra console để kiểm tra
                Console.WriteLine($"Received MovieScheduleId: {message.MovieScheduleId}");
                await Task.CompletedTask;
            });
            
            // Thêm một khoảng thời gian chờ ngắn để không làm quá tải CPU
            await Task.Delay(1000, stoppingToken);
        }
    }
}