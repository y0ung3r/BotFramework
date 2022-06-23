using System;
using System.Threading;
using System.Threading.Tasks;

namespace BotFramework;

internal class UpdateAwaiter
{
    private object _update;
    
    private readonly CancellationTokenSource _supplyCancellationTokenSource;
    
    public Type UpdateType { get; }
    
    public UpdateAwaiter(Type updateType)
    {
        _supplyCancellationTokenSource = new CancellationTokenSource();
        
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