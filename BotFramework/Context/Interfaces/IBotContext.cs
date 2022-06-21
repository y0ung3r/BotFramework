using System.Threading.Tasks;

namespace BotFramework.Context.Interfaces;

public interface IBotContext<TClient>
    where TClient : class
{
    public TClient Client { get; }
    
    Task<TUpdate> WaitNextUpdateAsync<TUpdate>()
        where TUpdate: class;
}