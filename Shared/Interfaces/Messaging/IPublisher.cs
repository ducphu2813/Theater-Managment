namespace Shared.Interfaces.Messaging;

public interface IPublisher<T> where T : class
{
    void Publish(T message);
}