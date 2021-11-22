using BotFramework.Attributes;
using System.Threading.Tasks;
using BotFramework.Handlers.Common.Interfaces;

namespace BotFramework.Tests.Fakes
{
    /// <summary>
    /// Fake для <see cref="ICommandHandler"/>
    /// </summary>
    [CommandAliases("/fake, /command")]
    public class FakeCommand : ICommandHandler
    {
        public Task HandleAsync(object request, RequestDelegate nextHandler) => Task.CompletedTask;
        
        public bool CanHandle(object request) => true;
    }

    /// <summary>
    /// Fake для <see cref="ICommandHandler"/> без использования псевдонима
    /// </summary>
    public class FakeCommandWithoutAliases : ICommandHandler
    {
        public Task HandleAsync(object request, RequestDelegate nextHandler) => Task.CompletedTask;
        
        public bool CanHandle(object request) => true;
    }
}
