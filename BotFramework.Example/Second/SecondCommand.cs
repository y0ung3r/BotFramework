using System;
using System.Threading.Tasks;
using BotFramework.Attributes;
using BotFramework.Extensions;
using BotFramework.Handlers;
using BotFramework.Handlers.Common;

namespace BotFramework.Example.Second
{
    [CommandAliases("/second")]
    internal class SecondCommand : CommandHandlerBase<string>
    {
        public override Task HandleAsync(string request, RequestDelegate nextHandler)
        {
            Console.WriteLine("Вызвана команда /second");

            return Task.CompletedTask;
        }

        public override bool CanHandle(string request)
        {
            return this.TextIsCommandAlias(request);
        }
    }
}