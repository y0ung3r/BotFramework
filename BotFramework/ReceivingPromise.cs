using System;
using System.Threading;
using System.Threading.Tasks;

namespace BotFramework;

public class ReceivingPromise
{
    private object _update;
    
    private readonly CancellationTokenSource _supplyCancellationTokenSource;
    
    public Type UpdateType { get; }
    
    public Type TargetType { get; }
    
    public ReceivingPromise(Type updateType, Type targetType)
    {
        _supplyCancellationTokenSource = new CancellationTokenSource();
        
        UpdateType = updateType;
        TargetType = targetType;
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
            WaitUpdate<TUpdate>,
            _supplyCancellationTokenSource.Token
        );
    }
    
    public void CancelWait()
    {
        _supplyCancellationTokenSource.Cancel();
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