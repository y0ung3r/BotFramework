using System;
using System.Threading.Tasks;
using BotFramework.Interfaces;

namespace BotFramework;

public class GreetingsHandler : IUpdateHandler<string>
{
    public async Task HandleAsync(string command, IBotContext context)
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