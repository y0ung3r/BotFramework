using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotFramework.Interfaces;

namespace BotFramework;

public class UpdatesMediator : IUpdateReceiver, IUpdateScheduler
{
    private readonly ICollection<ReceivingPromise> _promises;

    public UpdatesMediator()
    {
        _promises = new List<ReceivingPromise>();
    }
    
    private ReceivingPromise CreatePromise<TUpdate>() 
        where TUpdate : class
    {
        var updateType = typeof(TUpdate);
        var promise = new ReceivingPromise(updateType);
        _promises.Add(promise);
        
        return promise;
    }

    private async Task ReceiveAsync<TUpdate>(TUpdate update)
        where TUpdate : class
    {
        var updateType = update.GetType();
        var existingPromise = _promises.FirstOrDefault(promise => promise.UpdateType == updateType);

        if (existingPromise is null)
        {
            var handler = new GreetingsHandler();
            var typedUpdate = update.ToString();
            var botContext = new BotContext(this);
            await handler.HandleAsync(typedUpdate, botContext);
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

    public Task<TUpdate> ScheduleAsync<TUpdate>() 
        where TUpdate : class => CreatePromise<TUpdate>().WaitUpdateAsync<TUpdate>();
}