using BotFramework.Handlers.Interfaces;
using BotFramework.Interfaces;

namespace BotFramework.Context.Interfaces;

public interface IBotContextFactory<TClient>
    where TClient : class
{
    IBotContext<TClient> Create(IUpdateScheduler scheduler, IUpdateHandler handler);
}