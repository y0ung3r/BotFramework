using System.Threading.Tasks;
using BotFramework.Context.Interfaces;
using BotFramework.Handlers.Interfaces;
using BotFramework.Interfaces;

namespace BotFramework;

/// <summary>
/// Реализация для <see cref="IHandlerInvoker"/>
/// </summary>
/// <typeparam name="TClient">Тип внешней системы</typeparam>
public class HandlerInvoker<TClient> : IHandlerInvoker
	where TClient : class
{
	private readonly IBotContextFactory<TClient> _contextFactory;
	private readonly IUpdateHandlerProvider _handlerFactory;

	/// <summary>
	/// Инициализирует <see cref="HandlerInvoker{TClient}"/>
	/// </summary>
	/// <param name="contextFactory"></param>
	/// <param name="handlerFactory"></param>
	public HandlerInvoker(IBotContextFactory<TClient> contextFactory, IUpdateHandlerProvider handlerFactory)
	{
		_contextFactory = contextFactory;
		_handlerFactory = handlerFactory;
	}

	/// <summary>
	/// Выполняет проверку условия выполнения для указанного обработчика
	/// </summary>
	/// <param name="handler">Обработчик</param>
	/// <param name="update">Обновление</param>
	/// <typeparam name="TUpdate">Тип обновления</typeparam>
	private Task<bool> CheckPrerequisiteIfExists<TUpdate>(IUpdateHandler<TUpdate, TClient> handler, TUpdate update)
	{
		if (handler is IWithAsyncPrerequisite<TUpdate> prerequisite)
		{
			return prerequisite.CanHandleAsync(update);
		}

		return Task.FromResult(true);
	}

	/// <inheritdoc />
	public async Task InvokeAsync<TUpdate>(IUpdateScheduler scheduler, TUpdate update)
	{
		var handlers = _handlerFactory.GetAll<TUpdate, TClient>();

		foreach (var handler in handlers)
		{
			var botContext = _contextFactory.Create(scheduler);
                
			if (await CheckPrerequisiteIfExists(handler, update))
			{
				await handler.HandleAsync(update, botContext);
			}
		}
	}
}