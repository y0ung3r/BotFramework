using System.Threading.Tasks;
using BotFramework.Context.Interfaces;
using BotFramework.Handlers.Interfaces;

namespace BotFramework.MetadataAnalyzer.Tests.Handlers;

/// <summary>
/// Представляет тестовый обработчик
/// </summary>
public class TestHandler : IUpdateHandler<string, HandlerAnalyzerTests>, IWithAsyncPrerequisite<string, HandlerAnalyzerTests>
{
	/// <inheritdoc />
	public async Task HandleAsync(string update, IBotContext<HandlerAnalyzerTests> context)
	{
		await context.WaitNextUpdateAsync<string>();
	}
	
	/// <inheritdoc />
	public Task<bool> CanHandleAsync(string update, IBotContext<HandlerAnalyzerTests> context)
	{
		return Task.FromResult(update == "text");
	}
}