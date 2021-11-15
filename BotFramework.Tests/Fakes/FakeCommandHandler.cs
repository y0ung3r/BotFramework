using BotFramework.Attributes;
using System.Threading.Tasks;
using BotFramework.Abstractions;

namespace BotFramework.Tests.Fakes
{
    [CommandAliases("/fake, /command")]
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
