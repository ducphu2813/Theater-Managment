using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReservationService.Messaging.Interface;

namespace ReservationService.Messaging.Consumer;

public class PaymentConsumer<T> : IConsumer<T> where T : class
{
    private readonly RabbitMQSettings _settings;
    
    private IConnection? _connection;
    private IModel? _channel;
    
    public PaymentConsumer(RabbitMQSettings settings)
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
            queue: "payment_queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
        
        _channel.ExchangeDeclare(
            exchange: "payment_exchange",
            type: ExchangeType.Direct
        );
        
        _channel.QueueBind(
            queue: "payment_queue",
            exchange: "payment_exchange",
            routingKey: "payment_key"
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
        
        Console.WriteLine($" [*] Waiting for messages in payment_queue");
        
        _channel.BasicConsume(
            queue: "payment_queue",
            autoAck: false,  // Set autoAck to false to manually acknowledge the message after processing
            consumer: consumer
        );
    }
}