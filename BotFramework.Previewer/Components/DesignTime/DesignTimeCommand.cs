using BotFramework.Context.Interfaces;
using BotFramework.Handlers.Interfaces;

namespace BotFramework.Previewer.Components.DesignTime;

public class DesignTimeCommand : IUpdateHandler<string, object>, IWithAsyncPrerequisite<string>
{
	public async Task HandleAsync(string update, IBotContext<object> context)
	{
		await context.WaitNextUpdateAsync<string>();
	}

	public Task<bool> CanHandleAsync(string update)
	{
		return Task.FromResult(update == "/command");
	}
}