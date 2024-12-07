namespace Shared.Interfaces.Messaging;

public interface IConsumer<T> where T : class
{
    void Consume(Func<T, Task> onMessage);
}