using System.Text;
using System.Text.Json;
using Analysis.Domain.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Interfaces.Messaging;

namespace Analysis.Application.Messaging;

public class TicketConsumer<T> : IConsumer<T> where T : class
{
    private readonly RabbitMQSettings _settings;
    
    private IConnection? _connection;
    private IModel? _channel;
    
    public TicketConsumer(RabbitMQSettings settings)
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
                // Gọi callback để xử lý message
                await onMessage(message);
            }
            
            Console.WriteLine($" [x] Received {jsonMessage}");
            
            // Acknowledge that the message was successfully processed
            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            
            // Do something with the message here
        };
        
        Console.WriteLine($" [*] Waiting for messages in ticket_admin_queue");
        
        _channel.BasicConsume(
            queue: "ticket_admin_queue",
            autoAck: false,  // Set autoAck to false to manually acknowledge the message after processing
            consumer: consumer
        );
    }
}