using BotFramework.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using BotFramework.Abstractions;

namespace BotFramework
{
    /// <summary>
    /// Стандартная реализация для <see cref="IBranchBuilder"/>
    /// </summary>
    public class BranchBuilder : IBranchBuilder
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

            _logger?.LogInformation("Обработчик запроса добавлен в цепочку");

            return this;
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

            _logger?.LogInformation("Цепочка обязанностей построена");

            return rootHandler;
        }
    }
}
