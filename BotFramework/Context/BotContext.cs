using System.Threading.Tasks;
using BotFramework.Context.Interfaces;
using BotFramework.Interfaces;

namespace BotFramework.Context;

internal class BotContext<TClient> : IBotContext<TClient>
    where TClient : class
{
    private readonly IUpdateScheduler _scheduler;
    
    public TClient Client { get; }

    public BotContext(IUpdateScheduler scheduler, TClient client)
    {
        _scheduler = scheduler;
        
        Client = client;
    }

    public Task<TUpdate> WaitNextUpdateAsync<TUpdate>() 
        where TUpdate : class => _scheduler.ScheduleAsync<TUpdate>();
}