using BotFramework.Handlers.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly Stack<IRequestHandler> _handlersToExecute;
        private readonly IReadOnlyCollection<ICommandHandler> _neighborhoodCommands;

        /// <summary>
        /// Показывает запущен ли текущий пошаговый обработчик
        /// </summary>
        public bool IsRunning => _handlersToExecute.Any();

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="serviceProvider">Поставщик сервисов</param>
        /// <param name="head">Команда, которая запускает пошаговый обработчик</param>
        /// <param name="handlers">Обработчики, который необходимо выполнять пошагово</param>        
        public TransitionHandler(IServiceProvider serviceProvider, ICommandHandler head, IReadOnlyCollection<IRequestHandler> handlers)
        {
            _logger = serviceProvider.GetService<ILogger<TransitionHandler>>();
            _source = handlers;
            _head = head;
            _handlersToExecute = new Stack<IRequestHandler>();
            _neighborhoodCommands = GetNeighborhoodCommands(serviceProvider);
        }

        /// <summary>
        /// Возвращает соседние команды по отношению к текущей команде
        /// </summary>
        /// <param name="serviceProvider">Поставщик сервисов</param>
        private IReadOnlyCollection<ICommandHandler> GetNeighborhoodCommands(IServiceProvider serviceProvider)
        {
            var commands = serviceProvider.GetServices<ICommandHandler>();
            var anotherCommands = commands.Where
            (
                commandHandler => !commandHandler.GetType()
                                                 .Equals
                                                 (
                                                    _head.GetType()
                                                 )
            )
            .ToList();

            return new ReadOnlyCollection<ICommandHandler>(anotherCommands);
        }

        /// <summary>
        /// Проверяет занята ли родительская ветвь в текущий момент
        /// </summary>
        /// <param name="request">Запрос</param>
        public bool IsBranchBusy(object request) => _neighborhoodCommands.Any
        (
            anotherCommand => anotherCommand.CanHandle(request)
        );

        /// <summary>
        /// Обработать запрос
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="nextHandler">Следующий обработчик по цепочке</param>
        public Task HandleAsync(object request, RequestDelegate nextHandler)
        {
            if (IsBranchBusy(request) || !IsRunning)
            {
                _logger?.LogInformation("Восстанавливается исходное состояние пошагового обработчика");

                _handlersToExecute.Clear();
            }
            
            if (!IsRunning)
            {
                _logger?.LogInformation("Запуск пошагового обработчика и передача в него текущего запроса");

                foreach (var handler in _source)
                {
                    _handlersToExecute.Push(handler);
                }

                return _head.HandleAsync(request, nextHandler);
            }

            _logger?.LogInformation("Текущий запрос перенаправляется в активный пошаговый обработчик");

            return _handlersToExecute.Pop()
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
