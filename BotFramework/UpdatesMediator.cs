using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotFramework.Context.Interfaces;
using BotFramework.Handlers.Interfaces;
using BotFramework.Interfaces;

namespace BotFramework;

public class UpdatesMediator<TClient> : IUpdateReceiver, IUpdateScheduler
    where TClient : class
{
    private readonly IBotContextFactory<TClient> _contextFactory;
    private readonly IUpdateHandlerFactory<TClient> _handlerFactory;
    private readonly ICollection<ReceivingPromise> _promises;

    public UpdatesMediator(IBotContextFactory<TClient> contextFactory, IUpdateHandlerFactory<TClient> handlerFactory)
    {
        _contextFactory = contextFactory;
        _handlerFactory = handlerFactory;
        _promises = new List<ReceivingPromise>();
    }
    
    private ReceivingPromise CreatePromise<TUpdate>(IUpdateHandler handler) 
        where TUpdate : class
    {
        var updateType = typeof(TUpdate);
        var handlerType = handler.GetType();
        var promise = new ReceivingPromise(updateType, handlerType);
        _promises.Add(promise);
        
        return promise;
    }

    private async Task ReceiveAsync<TUpdate>(TUpdate update)
        where TUpdate : class
    {
        var updateType = update.GetType();
        var existingPromise = _promises.FirstOrDefault
        (
            promise => promise.UpdateType == updateType
        );

        if (existingPromise is null)
        {
            var handlers = _handlerFactory.Create<TUpdate>();

            foreach (var handler in handlers)
            {
                var botContext = _contextFactory.Create(this, handler);
                await handler.HandleAsync(update, botContext);
            }
        }
        else
        {
            existingPromise.Supply(update);
            _promises.Remove(existingPromise);
        }
    }

    public void Receive<TUpdate>(TUpdate update)
        where TUpdate : class
    {
        Task.Run
        (
            () => ReceiveAsync(update)
        );
    }

    public Task<TUpdate> ScheduleAsync<TUpdate>(IUpdateHandler handler) 
        where TUpdate : class => CreatePromise<TUpdate>(handler).WaitUpdateAsync<TUpdate>();
}