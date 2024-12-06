

using ReservationService.Core.Events;
using ReservationService.Core.Interfaces.Messaging;

namespace ReservationService.Application.Service.Background;

public class ScheduleConsumerService : BackgroundService
{
    private readonly IConsumer<MovieScheduleEvent> _consumer;
    
    public ScheduleConsumerService(IConsumer<MovieScheduleEvent> consumer)
    {
        _consumer = consumer;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            
            // chỉ gọi consume một lần khi service khởi chạy
            _consumer.Consume(onMessage: async (message) =>
            {
                // log message ra console để kiểm tra
                Console.WriteLine($"Received MovieScheduleId: {message.MovieScheduleId}");
                await Task.CompletedTask;
            });
            
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}