namespace ReservationService.Application.Messaging;

public class RabbitMQSettings
{
    public string HostName { get; set; } = "rabbitmq";
    public string UserName { get; set; } = "user";
    public string Password { get; set; } = "password";
    public string ExchangeName { get; set; } = "direct_exchange";
    
    // Danh sách các Queue và RoutingKey
    public List<QueueSettings> Queues { get; set; }
}

public class QueueSettings
{
    public string QueueName { get; set; }
    public string RoutingKey { get; set; }
}