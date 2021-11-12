using BotFramework.Handlers;
using BotFramework.Extensions;
using System;
using System.Threading.Tasks;
using BotFramework.Attributes;

namespace BotFramework.Example
{
    [CommandAliases("/another")]
    internal class AnotherCommand : CommandHandlerBase<string>
    {
        public override Task HandleAsync(string request, RequestDelegate nextHandler)
        {
            Console.WriteLine("Вызвана команда /another");

            return Task.CompletedTask;
        }

        public override bool CanHandle(string request)
        {
            return this.TextIsCommandAlias(request);
        }
    }
}