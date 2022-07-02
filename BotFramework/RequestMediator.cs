using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotFramework.Interfaces;

namespace BotFramework;

internal class RequestMediator : IUpdateReceiver, IUpdateScheduler
{
    private readonly IHandlerInvoker _handlerInvoker;
    private readonly ICollection<UpdateAwaiter> _awaiters;

    public RequestMediator(IHandlerInvoker handlerInvoker)
    {
        _handlerInvoker = handlerInvoker;
        
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
            await _handlerInvoker.InvokeAsync(this, update);
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