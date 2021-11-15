using System;
using System.Threading.Tasks;
using BotFramework.Handlers;

namespace BotFramework.Example.First
{
    internal class StartHandler : RequestHandlerBase<string>
    {
        public override Task HandleAsync(string request, RequestDelegate nextHandler)
        {
            Console.WriteLine($"Вы ввели текст: {request}");

            return Task.CompletedTask;
        }
    }
}