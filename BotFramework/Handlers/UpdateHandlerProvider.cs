using System.Collections.Generic;
using System.Linq;
using BotFramework.Handlers.Interfaces;

namespace BotFramework.Handlers;

public class UpdateHandlerProvider<TClient> : IUpdateHandlerProvider<TClient>
    where TClient : class
{
    private readonly IEnumerable<IUpdateHandler> _handlers;

    public UpdateHandlerProvider(IEnumerable<IUpdateHandler> handlers)
    {
        _handlers = handlers;
    }
    
    public IEnumerable<IUpdateHandler<TUpdate, TClient>> GetAll<TUpdate>() 
        where TUpdate : class => _handlers.OfType<IUpdateHandler<TUpdate, TClient>>();
}