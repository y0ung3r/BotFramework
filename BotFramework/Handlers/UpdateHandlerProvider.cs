using System.Collections.Generic;
using System.Linq;
using BotFramework.Handlers.Interfaces;

namespace BotFramework.Handlers;

/// <inheritdoc />
public class UpdateHandlerProvider : IUpdateHandlerProvider
{
    private readonly IEnumerable<IUpdateHandler> _handlers;

    /// <summary>
    /// Инициализирует <see cref="UpdateHandlerProvider"/>
    /// </summary>
    /// <param name="handlers">Зарегистрированные обработчики</param>
    public UpdateHandlerProvider(IEnumerable<IUpdateHandler> handlers)
    {
        _handlers = handlers;
    }
    
    /// <inheritdoc />
    public IEnumerable<IUpdateHandler<TUpdate, TClient>> GetAll<TUpdate, TClient>() 
        where TUpdate : class
        where TClient : class
    {
        return _handlers.OfType<IUpdateHandler<TUpdate, TClient>>();
    }
}