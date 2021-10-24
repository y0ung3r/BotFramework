using BotFramework.Extensions;
using BotFramework.Handlers;
using BotFramework.Handlers.Interfaces;
using BotFramework.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BotFramework
{
    /// <summary>
    /// Стандартная реализация для <see cref="IBranchBuilder"/>
    /// </summary>
    public class BranchBuilder : IBranchBuilder
    {
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
            _handlers = new Stack<IRequestHandler>();

            ServiceProvider = serviceProvider;
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

            return this;
        }

        /// <summary>
        /// Добавляет в цепочку отдельную ветвь и возвращает текущий экземпляр построителя цепочки обязанностей
        /// </summary>
        /// <param name="predicate">Условие, при котором происходит переход к добавляемой ветви при обработке запроса</param>
        /// <param name="configure">Конфигурация добавляемой ветви</param>
        /// <returns>Текущий экземпляр построителя цепочки обязанностей</returns>
        public IBranchBuilder UseAnotherBranch(Predicate<object> predicate, Action<IBranchBuilder> configure)
        {
            var anotherBranchBuilder = ServiceProvider.GetRequiredService<IBranchBuilder>();
            configure(anotherBranchBuilder);

            var anotherBranch = anotherBranchBuilder.Build();
            var internalHandlerFactory = ServiceProvider.GetRequiredService<Func<RequestDelegate, Predicate<object>, InternalHandler>>();

            return UseHandler
            (
                internalHandlerFactory(anotherBranch, predicate)
            );
        }

        /// <summary>
        /// Строит цепочку обязанностей
        /// </summary>
        /// <returns>Цепочка обязанностей</returns>
        public RequestDelegate Build()
        {
            var rootHandler = default(RequestDelegate);

            var branch = _handlers.Select
            (
                handler => new Func<RequestDelegate, RequestDelegate>
                (
                    next => handler.ToRequestDelegate(next)
                )
            );

            foreach (var handler in branch)
            {
                rootHandler = handler(rootHandler);
            }

            return rootHandler;
        }
    }
}
