using System;
using System.Threading.Tasks;
using BotFramework.Handlers;
using BotFramework.Handlers.Common;

namespace BotFramework.Example.Common
{
    internal class MissingRequestHandler : RequestHandlerBase<string>
    {
        public override Task HandleAsync(string request, RequestDelegate nextHandler)
        {
            Console.WriteLine("Невозможно обработать Ваш запрос");

            return Task.CompletedTask;
        }
    }
}