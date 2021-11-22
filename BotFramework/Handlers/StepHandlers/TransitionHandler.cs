using System;
using System.Collections.Generic;
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
        private readonly Func<ICommandHandler, IReadOnlyCollection<IStepHandler>, ITransitionContext> _contextFactory;
        private readonly ICommandHandler _command;
        private readonly IReadOnlyCollection<IStepHandler> _source;
        private readonly Func<object, object> _keySelector;
        private readonly IDictionary<object, ITransitionContext> _executingContexts;

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="logger">Сервис логгирования</param>
        /// <param name="contextFactory">Фабричный метод для создания <see cref="ITransitionContext"/></param>
        /// <param name="command">Команда, которая запускает пошаговые обработчики</param>
        /// <param name="handlers">Обработчики, который необходимо выполнять пошагово</param>
        /// <param name="uniqueKeySelector">Селектор уникального ключа из запроса (например, идентификатор чата)</param>
        public TransitionHandler(ILogger<TransitionHandler> logger, Func<ICommandHandler, IReadOnlyCollection<IStepHandler>, ITransitionContext> contextFactory, 
            ICommandHandler command, IReadOnlyCollection<IStepHandler> handlers, Func<object, object> uniqueKeySelector)
        {
            _logger = logger;
            _command = command;
            _contextFactory = contextFactory;
            _source = handlers;
            _keySelector = uniqueKeySelector;
            _executingContexts = new Dictionary<object, ITransitionContext>();
        }
        
        /// <summary>
        /// Проверяет могут ли пошаговые обработчики быть запущены, используя <see cref="_keySelector"/>
        /// </summary>
        /// <param name="request">Запрос</param>
        private bool NextStepHandlerCanHandle(object request)
        {
            var key = _keySelector(request);
            var containsKey = _executingContexts.ContainsKey(key);
            return containsKey && _executingContexts[key].IsRunning && _executingContexts[key].CanHandle(request);
        }

        /// <summary>
        /// Обработать запрос
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="nextHandler">Следующий обработчик по цепочке</param>
        public async Task HandleAsync(object request, RequestDelegate nextHandler)
        {
            _logger?.LogInformation("Выполнение запроса обработчиком пошаговых переходов");

            var key = _keySelector(request);

            if (!_executingContexts.ContainsKey(key))
            {
                _executingContexts.Add
                (
                    key,
                    _contextFactory(_command, _source)
                );
            }

            var context = _executingContexts[key];

            await context.HandleAsync(request, nextHandler)
                         .ConfigureAwait(false);
            
            if (!context.IsRunning)
            {
                _executingContexts.Remove(key);
            }
        }

        /// <summary>
        /// Проверяет могут ли пошаговые обработчики быть запущены
        /// </summary>
        /// <param name="request">Запрос</param>
        public bool CanHandle(object request) => _command.CanHandle(request) || NextStepHandlerCanHandle(request);
    }
}
