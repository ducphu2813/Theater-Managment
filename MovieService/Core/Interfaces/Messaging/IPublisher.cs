namespace MovieService.Core.Interfaces.Messaging;

public interface IPublisher<T> where T : class
{
    void Publish(T message);
}