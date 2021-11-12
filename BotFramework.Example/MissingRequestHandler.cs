using BotFramework.Handlers;
using System;
using System.Threading.Tasks;

namespace BotFramework.Example
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