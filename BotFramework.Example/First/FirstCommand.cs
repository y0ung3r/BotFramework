using System;
using System.Threading.Tasks;
using BotFramework.Attributes;
using BotFramework.Extensions;
using BotFramework.Handlers;
using BotFramework.Handlers.Common;

namespace BotFramework.Example.First
{
    [CommandAliases("/first")]
    internal class FirstCommand : CommandHandlerBase<string>
    {
        public override Task HandleAsync(string request, RequestDelegate nextHandler)
        {
            Console.WriteLine("Начало команды /first");

            return Task.CompletedTask;
        }

        public override bool CanHandle(string request)
        {
            return this.TextIsCommandAlias(request);
        }
    }
}