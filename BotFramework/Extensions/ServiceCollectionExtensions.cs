using BotFramework.Handlers;
using BotFramework.Handlers.Interfaces;
using BotFramework.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace BotFramework.Extensions
{
    /// <summary>
    /// Методы-расширения для <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Добавляет BotFramework в контейнер
        /// </summary>
        /// <param name="services">Контейнер</param>
        /// <returns>Контейнер</returns>
        public static IServiceCollection AddBotFramework(this IServiceCollection services)
        {
            services.AddLogging();

            services.TryAddTransient<IBranchBuilder, BranchBuilder>();

            services.TryAddTransient<IBotFactory, BotFactory>();

            services.TryAddTransient<Func<RequestDelegate, Predicate<object>, InternalHandler>>
            (
                serviceProvider => (branch, predicate) => ActivatorUtilities.CreateInstance<InternalHandler>(serviceProvider, branch, predicate)
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
