using System.Threading.Tasks;
using BotFramework.Abstractions;

namespace BotFramework.Tests.Fakes
{
    public class FakeRequestHandler : IRequestHandler
    {
        public Task HandleAsync(object request, RequestDelegate nextHandler) => Task.CompletedTask;
    }
}
