using System;
using System.Linq;
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
        services.TryAddTransient<IUpdateHandlerFactory<TClient>, UpdateHandlerFactory<TClient>>();
        services.TryAddSingleton<IUpdateReceiver, UpdatesMediator<TClient>>();
        services.TryAddSingleton<IUpdateScheduler, UpdatesMediator<TClient>>();

        return services;
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