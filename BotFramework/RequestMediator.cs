using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BotFramework.Extensions;
using BotFramework.Interfaces;

namespace BotFramework;

/// <summary>
/// Реализация для <see cref="IUpdateReceiver"/> и <see cref="IUpdateScheduler"/>
/// </summary>
internal class RequestMediator : IUpdateReceiver, IUpdateScheduler
{
    /// <summary>
    /// Запросы на ожидание обновлений
    /// </summary>
    internal IReadOnlyCollection<UpdateAwaiter> Awaiters => new ReadOnlyCollection<UpdateAwaiter>(_awaiters);

    private readonly IHandlerInvoker _handlerInvoker;
    private readonly IList<UpdateAwaiter> _awaiters;

    /// <summary>
    /// Инициализирует <see cref="RequestMediator"/>
    /// </summary>
    /// <param name="handlerInvoker">Реализация механизма запуска обработчиков</param>
    public RequestMediator(IHandlerInvoker handlerInvoker)
    {
        _handlerInvoker = handlerInvoker;
        
        _awaiters = new List<UpdateAwaiter>();
    }
    
    /// <summary>
    /// Создает запрос на ожидание обновления
    /// </summary>
    /// <typeparam name="TUpdate">Тип обновления</typeparam>
    private UpdateAwaiter CreateAwaiter<TUpdate>()
    {
        var updateType = typeof(TUpdate);
        var awaiter = new UpdateAwaiter(updateType);
        
        _awaiters.Add(awaiter);
        
        return awaiter;
    }

    /// <summary>
    /// Реализация для <see cref="Receive"/>
    /// </summary>
    /// <param name="update">Обновление</param>
    /// <typeparam name="TUpdate">Тип обновления</typeparam>
    private async Task ReceiveAsync<TUpdate>(TUpdate update)
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

    /// <summary>
    /// Используется для асинхронного запуска <see cref="ReceiveAsync{TUpdate}"/>
    /// </summary>
    /// <param name="update">Обновление</param>
    /// <typeparam name="TUpdate">Тип обновления</typeparam>
    private void Receive<TUpdate>(TUpdate update)
    {
        Task.Run
        (
            () => ReceiveAsync(update)
        );
    }

    /// <inheritdoc />
    public void Receive(object update)
    {
        this.InvokeReceive(update);
    }

    /// <inheritdoc />
    public Task<TUpdate> ScheduleAsync<TUpdate>()
    {
        var awaiter = CreateAwaiter<TUpdate>();
        return awaiter.WaitUpdateAsync<TUpdate>();
    }
}