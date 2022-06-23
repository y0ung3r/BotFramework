using BotFramework.Context.Interfaces;
using BotFramework.Interfaces;

namespace BotFramework.Context;

internal class BotContextFactory<TClient> : IBotContextFactory<TClient>
    where TClient : class
{
    private readonly TClient _client;
    
    public BotContextFactory(TClient client)
    {
        _client = client;
    }

    public IBotContext<TClient> Create(IUpdateScheduler scheduler)
    {
        return new BotContext<TClient>(scheduler, _client);
    }
}