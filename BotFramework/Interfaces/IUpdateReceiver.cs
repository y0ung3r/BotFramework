namespace BotFramework.Interfaces;

public interface IUpdateReceiver
{
    void Receive<TUpdate>(TUpdate update)
        where TUpdate : class;
}