using System.Threading.Tasks;
using BotFramework.Handlers.StepHandlers.Interfaces;

namespace BotFramework.Tests.Fakes
{
    /// <summary>
    /// Fake для <see cref="IStepHandler"/>
    /// </summary>
    public class FakeStepHandler : IStepHandler
    {
        public Task HandleAsync(object request, RequestDelegate nextHandler) => Task.CompletedTask;

        public bool CanHandle(object request) => true;

        public Task HandleAsync(object previousRequest, object currentRequest) => Task.CompletedTask;

        public bool CanHandle(object previousRequest, object currentRequest) => true;
    }
}