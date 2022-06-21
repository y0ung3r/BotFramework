using System.Threading.Tasks;
using BotFramework.Context.Interfaces;
using BotFramework.Handlers.Interfaces;
using BotFramework.Interfaces;

namespace BotFramework.Context;

internal class BotContext<TClient> : IBotContext<TClient>
    where TClient : class
{
    private readonly IUpdateScheduler _scheduler;
    private readonly IUpdateHandler _handler;
    
    public TClient Client { get; }

    public BotContext(IUpdateScheduler scheduler, IUpdateHandler handler, TClient client)
    {
        _scheduler = scheduler;
        _handler = handler;
        
        Client = client;
    }

    public Task<TUpdate> WaitNextUpdateAsync<TUpdate>() 
        where TUpdate : class => _scheduler.ScheduleAsync<TUpdate>(_handler);
}