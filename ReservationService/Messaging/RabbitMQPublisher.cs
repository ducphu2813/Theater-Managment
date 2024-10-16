﻿using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using ReservationService.Messaging.Interface;

namespace ReservationService.Messaging;

public class RabbitMQPublisher<T> : IPublisher<T> where T : class
{
    private readonly RabbitMQSettings _settings;
    
    private IConnection? _connection;
    private IModel? _channel;
    
    public RabbitMQPublisher(RabbitMQSettings settings)
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
            queue: "movie_schedule_queue",
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
            queue: "movie_schedule_queue",
            exchange: _settings.ExchangeName,
            routingKey: "movie_schedule_key"
        );
    }
    
    public void Publish(T message)
    {
        var jsonMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        _channel.BasicPublish(
            exchange: _settings.ExchangeName,
            routingKey: "movie_schedule_key",
            basicProperties: null,
            body: body
        );
    }
    
    public void Close()
    {
        _channel.Close();
        _connection.Close();
    }
    
}