using System.Collections.Generic;

namespace BotFramework.Handlers.Interfaces;

public interface IUpdateHandlerFactory<in TClient>
    where TClient : class
{
    public IEnumerable<IUpdateHandler<TUpdate, TClient>> Create<TUpdate>()
        where TUpdate : class;
}