using System;
using System.Threading;
using System.Threading.Tasks;

namespace BotFramework;

/// <summary>
/// Представляет запрос на ожидание обновление
/// </summary>
internal class UpdateAwaiter
{
    private object _update;
    
    private readonly CancellationTokenSource _supplyCancellationTokenSource;
    
    /// <summary>
    /// Тип обновления для которого запускается ожидание
    /// </summary>
    public Type UpdateType { get; }
    
    /// <summary>
    /// Инициализирует <see cref="UpdateAwaiter"/>
    /// </summary>
    /// <param name="updateType">Тип обновления</param>
    public UpdateAwaiter(Type updateType)
    {
        _supplyCancellationTokenSource = new CancellationTokenSource();
        
        UpdateType = updateType;
    }

    /// <summary>
    /// Поставляет обновление от внешней системы
    /// </summary>
    /// <param name="update">Обновление</param>
    /// <typeparam name="TUpdate">Тип обновления</typeparam>
    public void Supply<TUpdate>(TUpdate update)
    {
        _update = update;
    }

    /// <summary>
    /// Запускает текущий запрос
    /// </summary>
    /// <typeparam name="TUpdate">Тип обновления</typeparam>
    public Task<TUpdate> WaitUpdateAsync<TUpdate>()
    {
        return Task.Run
        (
            WaitUpdate<TUpdate>,
            _supplyCancellationTokenSource.Token
        );
    }
    
    /// <summary>
    /// Останавливает текущий запрос
    /// </summary>
    public void CancelWait()
    {
        _supplyCancellationTokenSource.Cancel();
    }

    /// <summary>
    /// Реализация для <see cref="WaitUpdate{TUpdate}"/>
    /// </summary>
    /// <typeparam name="TUpdate">Тип обновления</typeparam>
    private TUpdate WaitUpdate<TUpdate>()
    {
        while (_update == null)
        {
            // Ignore...
        }

        return (TUpdate) _update;
    }
}