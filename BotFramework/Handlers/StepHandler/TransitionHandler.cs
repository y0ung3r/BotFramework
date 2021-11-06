using BotFramework.Handlers.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotFramework.Handlers.StepHandler
{
    /// <summary>
    /// Представляет обработчик переходов
    /// </summary>
    internal sealed class TransitionHandler : ICommandHandler
    {
        private readonly ILogger<TransitionHandler> _logger;
        private readonly IReadOnlyCollection<IRequestHandler> _source;
        private readonly ICommandHandler _head;
        private readonly Stack<IRequestHandler> _handlers;

        /// <summary>
        /// Показывает запущен ли текущий пошаговый обработчик
        /// </summary>
        public bool IsRunning => _handlers.Any();

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="logger">Сервис логгирования</param>
        /// <param name="handlers">Обработчики, который необходимо выполнять пошагово</param>
        /// <param name="head">Команда, которая запускает пошаговый обработчик</param>
        public TransitionHandler(ILogger<TransitionHandler> logger, IReadOnlyCollection<IRequestHandler> handlers, ICommandHandler head)
        {
            _logger = logger;
            _source = handlers;
            _head = head;
            _handlers = new Stack<IRequestHandler>();
        }

        /// <summary>
        /// Обработать запрос
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="nextHandler">Следующий обработчик по цепочке</param>
        public Task HandleAsync(object request, RequestDelegate nextHandler)
        {
            if (!IsRunning)
            {
                _logger?.LogInformation("Запуск пошагового обработчика и передача в него текущего запроса");

                foreach (var handler in _source)
                {
                    _handlers.Push(handler);
                }

                return _head.HandleAsync(request, nextHandler);
            }

            _logger?.LogInformation("Текущий запрос перенаправляется в активный пошаговый обработчик");

            return _handlers.Pop()
                            .HandleAsync(request, nextHandler);
        }

        /// <summary>
        /// Проверяет может ли пошаговый обработчик быть запущен
        /// </summary>
        /// <param name="request">Запрос</param>
        public bool CanHandle(object request)
        {
            return _head.CanHandle(request) || IsRunning;
        }
    }
}
