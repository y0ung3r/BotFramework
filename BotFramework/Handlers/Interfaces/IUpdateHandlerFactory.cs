using System.Collections.Generic;

namespace BotFramework.Handlers.Interfaces;

public interface IUpdateHandlerFactory<TClient>
    where TClient : class
{
    public IEnumerable<IUpdateHandler<TUpdate, TClient>> Create<TUpdate>()
        where TUpdate : class;
}