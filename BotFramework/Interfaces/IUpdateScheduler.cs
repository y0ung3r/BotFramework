using System.Threading.Tasks;
using BotFramework.Handlers.Interfaces;

namespace BotFramework.Interfaces;

public interface IUpdateScheduler
{
    Task<TUpdate> ScheduleAsync<TUpdate>(IUpdateHandler handler)
        where TUpdate : class;
}