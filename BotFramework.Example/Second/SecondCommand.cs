using System;
using System.Threading.Tasks;
using BotFramework.Abstractions;
using BotFramework.Attributes;
using BotFramework.Extensions;

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