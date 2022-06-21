using System.Threading.Tasks;

namespace BotFramework.Interfaces;

public interface IBotContext
{
    Task<TUpdate> WaitNextUpdateAsync<TUpdate>()
        where TUpdate: class;
}