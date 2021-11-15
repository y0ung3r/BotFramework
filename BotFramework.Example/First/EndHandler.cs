using System;
using System.Threading.Tasks;
using BotFramework.Handlers;

namespace BotFramework.Example.First
{
    internal class EndHandler : RequestHandlerBase<string>
    {
        public override Task HandleAsync(string request, RequestDelegate nextHandler)
        {
            Console.WriteLine("Конец команды /first");

            return Task.CompletedTask;
        }
    }
}