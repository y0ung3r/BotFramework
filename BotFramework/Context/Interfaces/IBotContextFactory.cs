using BotFramework.Interfaces;

namespace BotFramework.Context.Interfaces;

public interface IBotContextFactory<out TClient>
    where TClient : class
{
    IBotContext<TClient> Create(IUpdateScheduler scheduler);
}