using BotFramework.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotFramework.StepHandler
{
    internal sealed class TransitionHandler : ICommandHandler
    {
        private readonly ILogger<TransitionHandler> _logger;
        private IReadOnlyCollection<IRequestHandler> _source;
        private readonly ICommandHandler _head;
        private readonly Stack<IRequestHandler> _handlers;

        public TransitionHandler(ILogger<TransitionHandler> logger, IReadOnlyCollection<IRequestHandler> handlers, ICommandHandler head)
        {
            _logger = logger;
            _source = handlers;
            _head = head;
            _handlers = new Stack<IRequestHandler>(_source);
        }

        public Task HandleAsync(object request, RequestDelegate nextHandler)
        {
            if (!_handlers.Any())
            {
                foreach (var handler in _source)
                {
                    _handlers.Push(handler);
                }

                return _head.HandleAsync(request, nextHandler);
            }

            return _handlers.Pop()
                            .HandleAsync(request, nextHandler);
        }

        public bool CanHandle(object request)
        {
            return _head.CanHandle(request) || _handlers.Any();
        }
    }
}
