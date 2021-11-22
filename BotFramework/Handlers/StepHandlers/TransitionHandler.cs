using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotFramework.Handlers.Common.Interfaces;
using BotFramework.Handlers.StepHandlers.Interfaces;
using BotFramework.Handlers.StepHandlers.States;
using Microsoft.Extensions.Logging;

namespace BotFramework.Handlers.StepHandlers
{
    /// <summary>
    /// Представляет обработчик пошаговых переходов
    /// </summary>
    internal sealed class TransitionHandler : ICommandHandler
    {
        private readonly ILogger<TransitionHandler> _logger;
        private readonly ICommandHandler _commandHandler;
        private readonly IReadOnlyCollection<IStepHandler> _source;
        private readonly Stack<IStepHandler> _handlersToExecute;
        private object _previousRequest;
        private TransitionHandlerStateBase _state;

        /// <summary>
        /// Показывает запущен ли текущий обработчик пошаговых переходов
        /// </summary>
        public bool IsRunning => _handlersToExecute.Any();

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="logger">Сервис логгирования</param>
        /// <param name="commandHandler">Команда, которая запускает пошаговые обработчики</param>
        /// <param name="handlers">Обработчики, который необходимо выполнять пошагово</param>
        /// <param name="uniqueKeySelector">Селектор уникального ключа из запроса (например, идентификатор чата)</param>
        public TransitionHandler(ILogger<TransitionHandler> logger, ICommandHandler commandHandler, 
            IReadOnlyCollection<IStepHandler> handlers, Func<object, object> uniqueKeySelector)
        {
            _logger = logger;
            _commandHandler = commandHandler;
            _source = handlers;
            _handlersToExecute = new Stack<IStepHandler>();
            
            SetState
            (
                new IdleState(this)
            );
        }

        /// <summary>
        /// Восстанавливает исходное состояние обработчиков
        /// </summary>
        private void RestoreStepHandlers()
        {
            _previousRequest = null;
            
            if (_handlersToExecute.Count > 0)
            {
                _handlersToExecute.Clear();
            }
            
            foreach (var handler in _source)
            {
                _handlersToExecute.Push(handler);
            }
        }
        
        /// <summary>
        /// Изменяет состояние текущего обработчика пошаговых переходов
        /// </summary>
        /// <param name="newState">Новое состояние</param>
        public void SetState(TransitionHandlerStateBase newState)
        {
            _state = newState;
        }
        
        /// <summary>
        /// Показывает может ли выполниться следующий пошаговый обработчик
        /// </summary>
        /// <param name="request">Запрос</param>
        public bool NextStepHandlerCanHandle(object request)
        {
            return IsRunning && _handlersToExecute.Peek().CanHandle(_previousRequest, request);
        }

        /// <summary>
        /// Показывает может ли выполниться команда, которая перезапустит выполнение пошаговых обработчиков
        /// </summary>
        /// <param name="request">Запрос</param>
        public bool CommandCanHandle(object request)
        {
            return _commandHandler.CanHandle(request);
        }

        /// <summary>
        /// Выполняет команду, которая запускает пошаговые обработчики
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="nextHandler">Следующий обработчик по цепочке</param>
        public Task HandleByCommandAsync(object request, RequestDelegate nextHandler)
        {
            _logger?.LogInformation("Запуск пошагового обработчика и передача в него текущего запроса");

            RestoreStepHandlers();

            return _commandHandler.HandleAsync(request, nextHandler);
        }

        /// <summary>
        /// Обрабатывает следующий пошаговый обработчик
        /// </summary>
        /// <param name="request">Запрос</param>
        public Task HandleByNextStepHandlerAsync(object request)
        {
            _logger?.LogInformation("Текущий запрос перенаправляется в активный пошаговый обработчик");

            return _handlersToExecute.Pop().HandleAsync(_previousRequest, request);
        }

        /// <summary>
        /// Обработать запрос
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="nextHandler">Следующий обработчик по цепочке</param>
        public async Task HandleAsync(object request, RequestDelegate nextHandler)
        {
            _logger?.LogInformation("Перенаправление запроса в обработчик пошаговых переходов");
            
            await _state.HandleAsync(request, nextHandler).ConfigureAwait(false);
            
            _previousRequest = request;
        }

        /// <summary>
        /// Проверяет могут ли пошаговые обработчики быть запущены
        /// </summary>
        /// <param name="request">Запрос</param>
        public bool CanHandle(object request) => _state.CanHandle(request);
    }
}
