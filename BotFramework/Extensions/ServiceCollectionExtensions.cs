using BotFramework.Handlers.Branches;
using BotFramework.Handlers.Interfaces;
using BotFramework.Handlers.StepHandler;
using BotFramework.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;

namespace BotFramework.Extensions
{
    /// <summary>
    /// Методы-расширения для <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Добавляет BotFramework и указанного бота в контейнер
        /// </summary>
        /// <param name="services">Контейнер</param>
        /// <returns>Контейнер</returns>
        public static IServiceCollection AddBotFramework(this IServiceCollection services)
        {
            services.TryAddSingleton
            (
                typeof(ILogger<>), 
                typeof(NullLogger<>)
            );

            services.TryAddTransient<IBranchBuilder, BranchBuilder>();

            services.TryAddTransient<Func<RequestDelegate, Predicate<object>, InternalHandler>>
            (
                serviceProvider => (branch, predicate) => ActivatorUtilities.CreateInstance<InternalHandler>(serviceProvider, branch, predicate)
            );

            services.TryAddTransient<Func<ICommandHandler, IReadOnlyCollection<IRequestHandler>, TransitionHandler>>
            (
                serviceProvider => (head, handlers) => ActivatorUtilities.CreateInstance<TransitionHandler>(serviceProvider, head, handlers)
            );

            return services;
        }

        /// <summary>
        /// Добавляет обработчик запроса в контейнер
        /// </summary>
        /// <typeparam name="TRequestHandler">Обработчик запроса, который необходимо добавить в контейнер</typeparam>
        /// <param name="services">Контейнер</param>
        /// <returns>Контейнер</returns>
        public static IServiceCollection AddHandler<TRequestHandler>(this IServiceCollection services)
            where TRequestHandler : IRequestHandler
        {
            services.TryAddTransient
            (
                typeof(TRequestHandler),
                typeof(TRequestHandler)
            );

            return services;
        }
    }
}
