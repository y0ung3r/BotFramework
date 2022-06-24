using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotFramework.Context.Interfaces;
using BotFramework.Handlers.Interfaces;
using BotFramework.Interfaces;

namespace BotFramework;

internal class RequestMediator<TClient> : IUpdateReceiver, IUpdateScheduler
    where TClient : class
{
    private readonly IBotContextFactory<TClient> _contextFactory;
    private readonly IUpdateHandlerFactory<TClient> _handlerFactory;
    private readonly ICollection<UpdateAwaiter> _awaiters;

    public RequestMediator(IBotContextFactory<TClient> contextFactory, IUpdateHandlerFactory<TClient> handlerFactory)
    {
        _contextFactory = contextFactory;
        _handlerFactory = handlerFactory;
        _awaiters = new List<UpdateAwaiter>();
    }
    
    private UpdateAwaiter CreateAwaiter<TUpdate>() 
        where TUpdate : class
    {
        var updateType = typeof(TUpdate);
        var awaiter = new UpdateAwaiter(updateType);
        
        _awaiters.Add(awaiter);
        
        return awaiter;
    }

    private Task<bool> CheckPrerequisiteIfExists<TUpdate>(IUpdateHandler<TUpdate, TClient> handler, TUpdate update)
        where TUpdate : class
    {
        if (handler is IWithAsyncPrerequisite<TUpdate> prerequisite)
        {
            return prerequisite.CanHandleAsync(update);
        }

        return Task.FromResult(true);
    }

    private async Task ReceiveAsync<TUpdate>(TUpdate update)
        where TUpdate : class
    {
        var updateType = update.GetType();
        var existingAwaiter = _awaiters.FirstOrDefault
        (
            awaiter => awaiter.UpdateType == updateType
        );

        if (existingAwaiter is null)
        {
            var handlers = _handlerFactory.Create<TUpdate>();

            foreach (var handler in handlers)
            {
                var botContext = _contextFactory.Create(this);
                
                if (await CheckPrerequisiteIfExists(handler, update))
                {
                    await handler.HandleAsync(update, botContext);
                }
            }
        }
        else
        {
            existingAwaiter.Supply(update);
            _awaiters.Remove(existingAwaiter);
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

    public Task<TUpdate> ScheduleAsync<TUpdate>() 
        where TUpdate : class
    {
        var awaiter = CreateAwaiter<TUpdate>();
        return awaiter.WaitUpdateAsync<TUpdate>();
    }
}