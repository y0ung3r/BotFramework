using System;
using System.Threading.Tasks;

namespace BotFramework;

public class ReceivingPromise
{
    private object _update;
    
    public Type UpdateType { get; }

    public ReceivingPromise(Type updateType)
    {
        UpdateType = updateType;
    }

    public void Supply<TUpdate>(TUpdate update)
        where TUpdate : class
    {
        _update = update;
    }

    public Task<TUpdate> WaitUpdateAsync<TUpdate>()
        where TUpdate : class
    {
        return Task.Run
        (
            WaitUpdate<TUpdate>
        );
    }

    private TUpdate WaitUpdate<TUpdate>()
        where TUpdate : class
    {
        
        while (_update == null)
        {
            // Ignore...
        }

        return _update as TUpdate;
    }
}