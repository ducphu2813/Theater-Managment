namespace AdminService.Messaging.Interface;

public interface IConsumer<T> where T : class
{
    void Consume(Func<T, Task> onMessage);
}