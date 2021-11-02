using BotFramework.Handlers.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BotFramework.Handlers
{
    internal class TransitionHandler : IRequestHandler
    {
        private readonly ILogger<TransitionHandler> _logger;
        private readonly RequestDelegate _from;
        private readonly RequestDelegate _to;

        public TransitionHandler(ILogger<TransitionHandler> logger, RequestDelegate from, RequestDelegate to)
        {
            _logger = logger;
            _from = from;
            _to = to;
        }

        public Task HandleAsync(object request, RequestDelegate nextHandler)
        {
            throw new System.NotImplementedException();
        }
    }
}
