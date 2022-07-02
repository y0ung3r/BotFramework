using System.Threading.Tasks;
using BotFramework.Context.Interfaces;
using BotFramework.Handlers.Interfaces;
using BotFramework.Interfaces;

namespace BotFramework;

public class HandlerInvoker<TClient> : IHandlerInvoker
	where TClient : class
{
	private readonly IBotContextFactory<TClient> _contextFactory;
	private readonly IUpdateHandlerProvider<TClient> _handlerFactory;

	public HandlerInvoker(IBotContextFactory<TClient> contextFactory, IUpdateHandlerProvider<TClient> handlerFactory)
	{
		_contextFactory = contextFactory;
		_handlerFactory = handlerFactory;
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

	public async Task InvokeAsync<TUpdate>(IUpdateScheduler scheduler, TUpdate update)
		where TUpdate : class
	{
		var handlers = _handlerFactory.GetAll<TUpdate>();

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