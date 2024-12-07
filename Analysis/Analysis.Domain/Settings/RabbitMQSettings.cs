namespace Analysis.Domain.Settings;

public class RabbitMQSettings
{
    public string HostName { get; set; } = "rabbitmq";
    public string UserName { get; set; } = "user";
    public string Password { get; set; } = "password";
    public string ExchangeName { get; set; } = "direct_exchange";
    public string QueueName { get; set; } = "default_queue";
    public string RoutingKey { get; set; } = "default_routing_key";
}