using System;
using BotFramework.Context;
using BotFramework.Context.Interfaces;
using BotFramework.Handlers;
using BotFramework.Handlers.Interfaces;
using BotFramework.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BotFramework.Extensions;

/// <summary>
/// Определяет методы-расширения для <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавляет BotFramework в контейнер зависимостей
    /// </summary>
    /// <param name="services">Контейнер зависимостей</param>
    /// <param name="clientFactory">Фабричный метод для получения внешней системы</param>
    /// <typeparam name="TClient">Тип внешней системы</typeparam>
    public static IServiceCollection AddBotFramework<TClient>(this IServiceCollection services, Func<IServiceProvider, TClient> clientFactory)
        where TClient : class
    {
        services.TryAddTransient(clientFactory);
        services.TryAddTransient<IBotContextFactory<TClient>, BotContextFactory<TClient>>();
        services.TryAddTransient<IUpdateHandlerProvider, UpdateHandlerProvider>();
        services.TryAddTransient<IHandlerInvoker, HandlerInvoker<TClient>>();
        services.TryAddSingleton<IUpdateReceiver, RequestMediator>();
        services.TryAddSingleton<IUpdateScheduler, RequestMediator>();

        return services;
    }

    /// <summary>
    /// Добавляет BotFramework в контейнер зависимостей, используя заранее зарегистированную внешнюю систему
    /// </summary>
    /// <param name="services">Контейнер зависимостей</param>
    /// <typeparam name="TClient">Тип внешней системы</typeparam>
    public static IServiceCollection AddBotFramework<TClient>(this IServiceCollection services)
        where TClient : class
    {
        return services.AddBotFramework
        (
            provider => provider.GetRequiredService<TClient>()
        );
    }

    /// <summary>
    /// Добавляет обработчик в контейнер зависимостей
    /// </summary>
    /// <param name="services">Контейнер зависимостей</param>
    /// <typeparam name="THandlerImplementation">Тип обработчика</typeparam>
    public static IServiceCollection AddHandler<THandlerImplementation>(this IServiceCollection services)
        where THandlerImplementation : IUpdateHandler
    {
        services.AddTransient
        (
            typeof(IUpdateHandler),
            typeof(THandlerImplementation)
        );

        return services;
    }
}