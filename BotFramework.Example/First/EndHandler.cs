using System;
using System.Threading.Tasks;
using BotFramework.Abstractions;

namespace BotFramework.Example.First
{
    internal class EndHandler : RequestHandlerBase<string>
    {
        public override Task HandleAsync(string request, RequestDelegate nextHandler)
        {
            Console.WriteLine($"Вы ввели текст: {request}");
            Console.WriteLine("Конец команды /first");

            return Task.CompletedTask;
        }
    }
}