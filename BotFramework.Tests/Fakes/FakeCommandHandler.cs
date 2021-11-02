using BotFramework.Attributes;
using BotFramework.Handlers.Interfaces;
using System;
using System.Threading.Tasks;

namespace BotFramework.Tests.Fakes
{
    [CommandText("/fake, /command")]
    public class FakeCommandHandler : ICommandHandler
    {
        public bool CanHandle(object request) => true;

        public Task HandleAsync(object request, RequestDelegate nextHandler) => Task.CompletedTask;
    }

    public class FakeCommandHandlerWithoutAliases : ICommandHandler
    {
        public bool CanHandle(object request) => true;

        public Task HandleAsync(object request, RequestDelegate nextHandler) => Task.CompletedTask;
    }
}
