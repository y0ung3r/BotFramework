using System.Threading.Tasks;

namespace BotFramework.Interfaces;

public interface IUpdateScheduler
{
    Task<TUpdate> ScheduleAsync<TUpdate>()
        where TUpdate : class;
}