using BotFramework.Attributes;
using BotFramework.Extensions;
using BotFramework.Handlers;
using System;
using System.Threading.Tasks;

namespace BotFramework.Example
{
    [CommandAliases("/bind")]
    internal class BindCommand : CommandHandlerBase<string>
    {
        public override Task HandleAsync(string request, RequestDelegate nextHandler)
        {
            Console.WriteLine("Начало команды /bind");

            return Task.CompletedTask;
        }

        public override bool CanHandle(string request)
        {
            return this.TextIsCommandAlias(request);
        }
    }
}