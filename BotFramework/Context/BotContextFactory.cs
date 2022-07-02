using BotFramework.Context.Interfaces;
using BotFramework.Interfaces;

namespace BotFramework.Context;

/// <inheritdoc cref="BotContextFactory{TClient}"/>
internal class BotContextFactory<TClient> : IBotContextFactory<TClient>
    where TClient : class
{
    private readonly TClient _client;
    
    /// <summary>
    /// Базовый конструктор
    /// </summary>
    /// <param name="client">Внешняя система</param>
    public BotContextFactory(TClient client)
    {
        _client = client;
    }

    /// <inheritdoc />
    public IBotContext<TClient> Create(IUpdateScheduler scheduler)
    {
        return new BotContext<TClient>(scheduler, _client);
    }
}