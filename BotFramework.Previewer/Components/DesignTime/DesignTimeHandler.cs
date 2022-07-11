using BotFramework.Context.Interfaces;
using BotFramework.Handlers.Interfaces;

namespace BotFramework.Previewer.Components.DesignTime;

public class DesignTimeHandler : IUpdateHandler<string, object>
{
	public async Task HandleAsync(string update, IBotContext<object> context)
	{
		await context.WaitNextUpdateAsync<string>();
		await context.WaitNextUpdateAsync<object>();
	}
}