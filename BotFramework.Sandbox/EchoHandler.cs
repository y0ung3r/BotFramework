using BotFramework.Context.Interfaces;
using BotFramework.Handlers.Interfaces;
using BotFramework.Interfaces;
using Telegram.Bot;

namespace BotFramework.Sandbox;

public class EchoHandler : IUpdateHandler<string, ITelegramBotClient>
{
    public async Task HandleAsync(string echo, IBotContext<ITelegramBotClient> context)
    {
        if (echo == "/echo")
        {
            var text = await context.WaitNextUpdateAsync<string>();
            Console.WriteLine(text);
        }
    }
}