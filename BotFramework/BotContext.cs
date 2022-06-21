using System.Threading.Tasks;
using BotFramework.Interfaces;

namespace BotFramework;

public class BotContext : IBotContext
{
    private readonly IUpdateScheduler _scheduler;

    public BotContext(IUpdateScheduler scheduler)
    {
        _scheduler = scheduler;
    }
    
    public Task<TUpdate> WaitNextUpdateAsync<TUpdate>() 
        where TUpdate : class => _scheduler.ScheduleAsync<TUpdate>();
}