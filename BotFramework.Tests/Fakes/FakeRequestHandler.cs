using System.Threading.Tasks;
using BotFramework.Handlers.Common.Interfaces;

namespace BotFramework.Tests.Fakes
{
    /// <summary>
    /// Fake для <see cref="IRequestHandler"/>
    /// </summary>
    public class FakeRequestHandler : IRequestHandler
    {
        public Task HandleAsync(object request, RequestDelegate nextHandler) => Task.CompletedTask;
    }
}
