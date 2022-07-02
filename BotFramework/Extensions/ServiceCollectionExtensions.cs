using System;
using BotFramework.Context;
using BotFramework.Context.Interfaces;
using BotFramework.Handlers;
using BotFramework.Handlers.Interfaces;
using BotFramework.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BotFramework.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBotFramework<TClient>(this IServiceCollection services, Func<IServiceProvider, TClient> clientFactory)
        where TClient : class
    {
        services.TryAddTransient(clientFactory);
        services.TryAddTransient<IBotContextFactory<TClient>, BotContextFactory<TClient>>();
        services.TryAddTransient<IUpdateHandlerProvider<TClient>, UpdateHandlerProvider<TClient>>();
        services.TryAddTransient<IHandlerInvoker, HandlerInvoker<TClient>>();
        services.TryAddSingleton<IUpdateReceiver, RequestMediator>();
        services.TryAddSingleton<IUpdateScheduler, RequestMediator>();

        return services;
    }

    public static IServiceCollection AddBotFramework<TClient>(this IServiceCollection services)
        where TClient : class
    {
        return services.AddBotFramework
        (
            provider => provider.GetRequiredService<TClient>()
        );
    }

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