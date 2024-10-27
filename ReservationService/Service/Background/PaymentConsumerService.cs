using ReservationService.Entity.Model;
using ReservationService.Events;
using ReservationService.Exceptions;
using ReservationService.External.Model;
using ReservationService.Messaging.Interface;
using ReservationService.Repository.Interface;
using ReservationService.Service.Interface;

namespace ReservationService.Service.Background;

public class PaymentConsumerService : BackgroundService
{
    private readonly IConsumer<PaymentEvent> _consumer;
    private readonly ITicketRepository _ticketRepository;
    private readonly ITicketService _ticketService;
    private readonly IPublisher<AdminEvent> _publisher;
    
    public PaymentConsumerService(IConsumer<PaymentEvent> consumer,
                                    ITicketRepository ticketRepository,
                                    ITicketService ticketService,
                                    IPublisher<AdminEvent> publisher)
    {
        _consumer = consumer;
        _ticketRepository = ticketRepository;
        _ticketService = ticketService;
        _publisher = publisher;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            
            // chỉ gọi consume một lần khi service khởi chạy
            _consumer.Consume(onMessage: async (message) =>
            {
                // log message ra console để kiểm tra
                Console.WriteLine($"Received PaymentId: {message.PaymentId}");
                Console.WriteLine($"Received Payment Status: {message.Status}");
                Console.WriteLine($"Received TicketId: {message.TicketId}");
                Console.WriteLine($"Received Payment Method: {message.PaymentMethod}");
                
                //sau đó thực hiện chỉnh sửa dữ liệu trong database
                //kiểm tra ticket tồn tại
                Ticket ticket = await _ticketRepository.GetById(message.TicketId);
                
                if (ticket == null)
                {
                    Console.WriteLine($"Ticket with id {message.TicketId} was not found.");
                }
                else
                {
                    //sau đó cập nhật Status cho Ticket theo id, và cập nhật ExpiryDate là null để tránh bị xóa bởi ttl
                    ticket.Status = message.Status;
                    ticket.ExpiryTime = null;
                    await _ticketRepository.Update(message.TicketId, ticket);
                    
                    //sau đó gửi ticket này đến queue đến admin service
                    //gửi những cái như sau: - User Id, Ticket Id, Created Date, BaseAmount, TotalAmount
                    //                       - Genres, ShowTime, movie
                    //                       - PaymentMethod, Payment Id
                    
                    //lấy chi tiết ticket
                    var ticketDetail = await _ticketService.GetByIdAsync(message.TicketId);
                    
                    //lấy ticket
                    Ticket extractedTicket = ticketDetail["ticket"] as Ticket;
                    
                    //lấy movie schedule ra
                    MovieSchedule movieSchedule = ticketDetail["movieSchedule"] as MovieSchedule;
                    
                    //gửi message
                    _publisher.Publish(new AdminEvent
                    {
                        UserId = extractedTicket.UserId,
                        TicketId = extractedTicket.Id,
                        PaymentId = message.PaymentId,
                        PaymentMethod = message.PaymentMethod,
                        ShowTime = movieSchedule.ShowTime,
                        BaseAmount = extractedTicket.BaseAmount,
                        TotalAmount = extractedTicket.TotalAmount,
                        Genres = movieSchedule.Movie.Genres,
                        SeatDetail = extractedTicket.SeatDetail,
                        FoodDetail = extractedTicket.FoodDetail,
                        DiscountDetail = extractedTicket.DiscountDetail,
                        MovieDetail = movieSchedule.Movie,
                        CreatedAt = extractedTicket.CreatedAt
                    });
                    
                }
                
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