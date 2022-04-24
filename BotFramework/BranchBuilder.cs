using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using BotFramework.Handlers.Common.Interfaces;
using BotFramework.Handlers.Extensions;
using BotFramework.Handlers.StepHandlers.Interfaces;
using BotFramework.Interfaces;

namespace BotFramework
{
    /// <summary>
    /// Стандартная реализация для <see cref="IBranchBuilder"/>
    /// </summary>
    public class BranchBuilder : IBranchBuilder, IStepsBuilder
    {
        private readonly ILogger<BranchBuilder> _logger;
        private readonly Stack<IRequestHandler> _handlers;

        /// <summary>
        /// Поставщик сервисов
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Список обработчиков, добавленных в цепочку 
        /// </summary>
        public IReadOnlyCollection<IRequestHandler> Handlers => _handlers.ToList().AsReadOnly();

        /// <summary>
        /// Конструктор для <see cref="BranchBuilder"/>
        /// </summary>
        /// <param name="serviceProvider">Поставщик сервисов</param>
        public BranchBuilder(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;

            _logger = ServiceProvider.GetService<ILogger<BranchBuilder>>();
            _handlers = new Stack<IRequestHandler>();
        }

        /// <summary>
        /// Конструктор для <see cref="BranchBuilder"/>
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        public BranchBuilder(IServiceCollection services)
            : this(
                services.BuildServiceProvider()
            )
        { }

        /// <summary>
        /// Добавляет в цепочку обработчик запроса и возвращает текущий экземпляр построителя цепочки обязанностей
        /// </summary>
        /// <param name="handler">Обработчик запроса, который необходимо добавить в цепочку</param>
        /// <returns>Текущий экземпляр построителя цепочки обязанностей</returns>
        public IBranchBuilder UseHandler(IRequestHandler handler)
        {
            _handlers.Push(handler);

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation
                (
                    "Обработчик запроса с типом \"{HandlerType}\" добавлен в цепочку", 
                    handler.GetType()
                );
            }

            return this;
        }
        
        /// <summary>
        /// Добавляет пошаговый обработчик в цепочку
        /// </summary>
        /// <param name="handler">Пошаговый обработчик</param>
        public IStepsBuilder UseStepHandler(IStepHandler handler)
        {
            return (IStepsBuilder)UseHandler(handler);
        }

        /// <summary>
        /// Строит цепочку обязанностей
        /// </summary>
        /// <returns>Цепочка обязанностей</returns>
        public RequestDelegate Build()
        {
            var rootHandler = _handlers.Select
            (
                handler => new Func<RequestDelegate, RequestDelegate>(handler.ToRequestDelegate)
            )
            .Aggregate
            (
                default(RequestDelegate), 
                (nextHandler, currentHandler) => currentHandler(nextHandler)
            );
            
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Цепочка обязанностей построена");
            }

            return rootHandler;
        }
    }
}
