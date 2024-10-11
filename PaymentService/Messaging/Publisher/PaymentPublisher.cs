using System.Text;
using System.Text.Json;
using PaymentService.Messaging.Interface;
using RabbitMQ.Client;

namespace PaymentService.Messaging.Publisher;

public class PaymentPublisher<T> : IPublisher<T> where T : class
{
    
    private readonly RabbitMQSettings _settings;
    
    private IConnection? _connection;
    private IModel? _channel;
    
    public PaymentPublisher(RabbitMQSettings settings)
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
            queue: _settings.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
        
        _channel.ExchangeDeclare(
            exchange: _settings.ExchangeName,
            type: ExchangeType.Direct
        );
        
        _channel.QueueBind(
            queue: _settings.QueueName,
            exchange: _settings.ExchangeName,
            routingKey: _settings.RoutingKey
        );
    }
    
    public void Publish(T message)
    {
        var jsonMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        _channel.BasicPublish(
            exchange: _settings.ExchangeName,
            routingKey: _settings.RoutingKey,
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