using Analysis.Domain.Entity;
using Analysis.Domain.Event;
using Analysis.Domain.Interfaces;
using Microsoft.Extensions.Hosting;
using Shared.Interfaces.Messaging;

namespace Analysis.Application.Service.Background;


public class TicketConsumService : BackgroundService
{
    
    private readonly IConsumer<TicketProcessedConsume> _consumer;
    private readonly IMovieSaleRepository _movieSaleRepository;
    
    public TicketConsumService(IConsumer<TicketProcessedConsume> consumer,
                                IMovieSaleRepository movieSaleRepository)
    {
        _consumer = consumer;
        _movieSaleRepository = movieSaleRepository;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // chỉ gọi consume một lần khi service khởi chạy
            _consumer.Consume(onMessage: async (message) =>
            {
                // xử lý message nhận được từ reservation service
                
                //in ra console
                Console.WriteLine($"TicketId: {message}");
                
                //sau đó lưu MovieSale vào database
                
                MovieSale movieSale = new MovieSale()
                {
                    UserId = message.UserId,
                    TicketId = message.TicketId,
                    PaymentId = message.PaymentId,
                    MovieDetail = message.MovieDetail,
                    PaymentMethod = message.PaymentMethod,
                    ShowTime = message.ShowTime,
                    SeatDetail = message.SeatDetail,
                    FoodDetail = message.FoodDetail,
                    DiscountDetail = message.DiscountDetail,
                    BaseAmount = message.BaseAmount,
                    TotalAmount = message.TotalAmount,
                    Genres = message.Genres,
                    TicketCreatedDate = message.CreatedAt
                };
                
                //lưu vào database
                await _movieSaleRepository.Add(movieSale);
                
                await Task.CompletedTask;
            });

            // vòng lặp này chỉ để giữ cho background service tiếp tục chạy
            while (!stoppingToken.IsCancellationRequested)
            {
                // chờ 1 giây để tránh quá tải CPU
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}