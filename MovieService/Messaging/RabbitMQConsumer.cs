using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MovieService.Messaging;

public class RabbitMQConsumer<T> : IConsumer<T> where T : class
{
    private readonly RabbitMQSettings _settings;
    
    private IConnection? _connection;
    private IModel? _channel;
    
    public RabbitMQConsumer(RabbitMQSettings settings)
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
    
    public void Consume(Func<T, Task> onMessage)
    {
        var consumer = new EventingBasicConsumer(_channel);
        
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var jsonMessage = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<T>(jsonMessage);
            
            if (message != null)
            {
                // gọi callback để xử lý message
                await onMessage(message);
            }
            
            Console.WriteLine($" [x] Received {jsonMessage}");
            
            // Acknowledge that the message was successfully processed
            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            
            // Do something with the message here
        };
        
        Console.WriteLine($" [*] Waiting for messages in {_settings.QueueName}");
        
        _channel.BasicConsume(
            queue: _settings.QueueName,
            autoAck: false,  // Set autoAck to false to manually acknowledge the message after processing
            consumer: consumer
        );
    }
    
}