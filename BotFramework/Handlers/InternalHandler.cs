using BotFramework.Handlers.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BotFramework.Handlers
{
    /// <summary>
    /// Представляет обработчик, который перенаправляет запрос из одной ветки обработчиков в другую
    /// </summary>
    internal class InternalHandler : IRequestHandler
    {
        private readonly ILogger<InternalHandler> _logger;
        private readonly RequestDelegate _branchRootHandler;
        private readonly Predicate<object> _predicate;

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="logger">Сервис логгирования</param>
        /// <param name="branch">Цепочка обработчиков</param>
        /// <param name="predicate">Условие, при выполнении которого должен происходить переход в цепочку обработчиков</param>
        public InternalHandler(ILogger<InternalHandler> logger, RequestDelegate branch, Predicate<object> predicate)
        {
            _logger = logger;
            _branchRootHandler = branch;
            _predicate = predicate;
        }

        public Task HandleAsync(object request, RequestDelegate nextHandler)
        {
            if (_predicate(request))
            {
                _logger?.LogInformation("A request will be redirected to the branch");

                return _branchRootHandler(request);
            }

            _logger?.LogInformation("A request will be redirected to the next handler");

            return nextHandler(request);
        }
    }
}