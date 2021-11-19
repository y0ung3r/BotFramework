using System;
using System.Threading.Tasks;
using BotFramework.Attributes;
using BotFramework.Handlers.Common;
using BotFramework.Handlers.Extensions;

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