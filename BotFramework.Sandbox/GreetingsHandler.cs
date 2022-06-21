using BotFramework.Context.Interfaces;
using BotFramework.Handlers.Interfaces;
using BotFramework.Interfaces;
using Telegram.Bot;

namespace BotFramework.Sandbox;

public class GreetingsHandler : IUpdateHandler<string, ITelegramBotClient>
{
    public async Task HandleAsync(string command, IBotContext<ITelegramBotClient> context)
    {
        if (command == "/greetings")
        {
            Console.WriteLine("Введите фамилию:");
            var lastname = await context.WaitNextUpdateAsync<string>();
            
            Console.WriteLine("Введите имя:");
            var firstname = await context.WaitNextUpdateAsync<string>();
            
            Console.WriteLine($"Привет, {lastname} {firstname}!");
        }
    }
}