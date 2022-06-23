using System.Threading.Tasks;

namespace BotFramework.Context.Interfaces;

public interface IBotContext<out TClient>
    where TClient : class
{
    public TClient Client { get; }
    
    Task<TUpdate> WaitNextUpdateAsync<TUpdate>()
        where TUpdate: class;
}