using System;
using System.Collections.Generic;
using System.Linq;
using BotFramework.Handlers.Interfaces;

namespace BotFramework.Handlers;

public class UpdateHandlerFactory<TClient> : IUpdateHandlerFactory<TClient>
    where TClient : class
{
    private readonly TClient _client;
    private readonly IEnumerable<IUpdateHandler> _handlers;

    public UpdateHandlerFactory(TClient client, IEnumerable<IUpdateHandler> handlers)
    {
        _client = client;
        _handlers = handlers;
    }
    
    public IEnumerable<IUpdateHandler<TUpdate, TClient>> Create<TUpdate>() 
        where TUpdate : class => _handlers.OfType<IUpdateHandler<TUpdate, TClient>>();
}