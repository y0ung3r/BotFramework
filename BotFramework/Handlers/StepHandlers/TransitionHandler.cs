using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotFramework.Handlers.Common.Interfaces;
using BotFramework.Handlers.StepHandlers.Interfaces;
using Microsoft.Extensions.Logging;

namespace BotFramework.Handlers.StepHandlers
{
    /// <summary>
    /// Представляет обработчик пошаговых переходов
    /// </summary>
    internal sealed class TransitionHandler : ICommandHandler
    {
        private readonly ILogger<TransitionHandler> _logger;
        private readonly ICommandHandler _head;
        private readonly IReadOnlyCollection<IStepHandler> _source;
        private readonly Stack<IStepHandler> _handlersToExecute;
        private object _previousRequest;

        /// <summary>
        /// Показывает запущен ли текущий обработчик пошаговых переходов
        /// </summary>
        public bool IsRunning => _handlersToExecute.Any();

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="logger">Сервис логгирования</param>
        /// <param name="head">Команда, которая запускает пошаговые обработчики</param>
        /// <param name="handlers">Обработчики, который необходимо выполнять пошагово</param>        
        public TransitionHandler(ILogger<TransitionHandler> logger, ICommandHandler head, IReadOnlyCollection<IStepHandler> handlers)
        {
            _logger = logger;
            _head = head;
            _source = handlers;
            _handlersToExecute = new Stack<IStepHandler>();
        }

        /// <summary>
        /// Выполняет команду, которая запускает пошаговые обработчики
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="nextHandler">Следующий обработчик по цепочке</param>
        private Task HandleHeadAsync(object request, RequestDelegate nextHandler)
        {
            _logger?.LogInformation("Запуск пошагового обработчика и передача в него текущего запроса");

            _previousRequest = null;
                
            foreach (var handler in _source)
            {
                _handlersToExecute.Push(handler);
            }

            return _head.HandleAsync(request, nextHandler);
        }

        /// <summary>
        /// Обрабатывает следующий пошаговый обработчик
        /// </summary>
        /// <param name="request">Запрос</param>
        private Task HandleStepAsync(object request)
        {
            _logger?.LogInformation("Текущий запрос перенаправляется в активный пошаговый обработчик");

            return _handlersToExecute.Pop()
                                     .HandleAsync(_previousRequest, request);
        }

        /// <summary>
        /// Обработать запрос
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="nextHandler">Следующий обработчик по цепочке</param>
        public async Task HandleAsync(object request, RequestDelegate nextHandler)
        {
            if (!IsRunning)
            {
                await HandleHeadAsync(request, nextHandler);
            }
            else
            {
                await HandleStepAsync(request);
            }
            
            _previousRequest = request;
        }

        /// <summary>
        /// Проверяет могут ли пошаговые обработчики быть запущены
        /// </summary>
        /// <param name="request">Запрос</param>
        public bool CanHandle(object request)
        {
            if (!IsRunning)
            {
                return _head.CanHandle(request);
            }

            return _handlersToExecute.Peek()
                                     .CanHandle(_previousRequest, request);
        }
    }
}
