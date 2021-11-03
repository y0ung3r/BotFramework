using BotFramework.Interfaces;
using System.Threading.Tasks;

namespace BotFramework.Tests.Fakes
{
    public class FakeRequestHandler : IRequestHandler
    {
        public Task HandleAsync(object request, RequestDelegate nextHandler) => Task.CompletedTask;
    }
}
