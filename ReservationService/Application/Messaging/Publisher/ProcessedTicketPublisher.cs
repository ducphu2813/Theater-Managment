using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using ReservationService.Core.Interfaces.Messaging;

namespace ReservationService.Application.Messaging.Publisher;

public class ProcessedTicketPublisher<T> : IPublisher<T> where T : class
{
    
    private readonly RabbitMQSettings _settings;
    
    private IConnection? _connection;
    private IModel? _channel;
    
    public ProcessedTicketPublisher(RabbitMQSettings settings)
    {
        _settings = settings;
        InitializeRabbitMQ();
    }
    
    private void InitializeRabbitMQ()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _settings.HostName,
            UserName = _settings.UserName,
            Password = _settings.Password
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(
            queue: "ticket_admin_queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
        
        _channel.ExchangeDeclare(
            exchange: "ticket_admin_exchange",
            type: ExchangeType.Direct
        );
        
        _channel.QueueBind(
            queue: "ticket_admin_queue",
            exchange: "ticket_admin_exchange",
            routingKey: "ticket_admin_key"
        );
    }
    
    public void Publish(T message)
    {
        var jsonMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        _channel.BasicPublish(
            exchange: "ticket_admin_exchange",
            routingKey: "ticket_admin_key",
            basicProperties: null,
            body: body
        );

        Console.WriteLine($" [x] Sent {jsonMessage}");
    }
    
    public void Close()
    {
        _channel.Close();
        _connection.Close();
    }
    
}