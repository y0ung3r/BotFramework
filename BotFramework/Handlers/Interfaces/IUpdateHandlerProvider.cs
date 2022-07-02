using System.Collections.Generic;

namespace BotFramework.Handlers.Interfaces;

public interface IUpdateHandlerProvider<in TClient>
    where TClient : class
{
    public IEnumerable<IUpdateHandler<TUpdate, TClient>> GetAll<TUpdate>()
        where TUpdate : class;
}