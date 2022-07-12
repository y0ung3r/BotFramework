namespace Previewer;

/// <summary>
/// Простой обработчик
/// </summary>
public class SimpleHandler : IUpdateHandler<string, TextWriter>, IWithAsyncPrerequisite<string, TextWriter>
{
	public async Task HandleAsync(string update, IBotContext<TextWriter> context)
	{
		Console.WriteLine($"Обновление получено: {update}");
		
		await context.WaitNextUpdateAsync<string>();
	}

	public Task CanHandleAsync(string update, IBotContext<TextWriter> context)
	{
		return Task.FromResult(update == "text");
	}
}