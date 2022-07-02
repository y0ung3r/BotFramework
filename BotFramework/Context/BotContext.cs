using System.Threading.Tasks;
using BotFramework.Context.Interfaces;
using BotFramework.Interfaces;

namespace BotFramework.Context;

/// <inheritdoc />
internal class BotContext<TClient> : IBotContext<TClient>
    where TClient : class
{
    private readonly IUpdateScheduler _scheduler;
    
    /// <inheritdoc />
    public TClient Client { get; }

    /// <summary>
    /// Инициализирует <see cref="BotContext{TClient}"/>
    /// </summary>
    /// <param name="scheduler">Планировщик для ожидания новых обновлений от внешней системы</param>
    /// <param name="client">Внешняя система</param>
    public BotContext(IUpdateScheduler scheduler, TClient client)
    {
        _scheduler = scheduler;
        
        Client = client;
    }

    /// <inheritdoc />
    public Task<TUpdate> WaitNextUpdateAsync<TUpdate>() 
        where TUpdate : class => _scheduler.ScheduleAsync<TUpdate>();
}