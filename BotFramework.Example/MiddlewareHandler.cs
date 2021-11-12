using BotFramework.Handlers;
using System;
using System.Threading.Tasks;

namespace BotFramework.Example
{
    internal class MiddlewareHandler : RequestHandlerBase<string>
    {
        public override Task HandleAsync(string request, RequestDelegate nextHandler)
        {
            Console.WriteLine($"Вы ввели текст: {request}");

            return Task.CompletedTask;
        }
    }
}