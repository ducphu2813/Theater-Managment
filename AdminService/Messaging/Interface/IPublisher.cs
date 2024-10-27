namespace AdminService.Messaging.Interface;

public interface IPublisher<T> where T : class
{
    void Publish(T message);
}