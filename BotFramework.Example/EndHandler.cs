using BotFramework.Handlers;
using System;
using System.Threading.Tasks;

namespace BotFramework.Example
{
    internal class EndHandler : RequestHandlerBase<string>
    {
        public override Task HandleAsync(string request, RequestDelegate nextHandler)
        {
            Console.WriteLine("Конец команды /bind");

            return Task.CompletedTask;
        }
    }
}