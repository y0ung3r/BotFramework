using System.Collections.Generic;
using System.Linq;
using BotFramework.Handlers.Interfaces;
using BotFramework.Previewer.HandlersMetadata;
using BotFramework.Previewer.HandlersMetadata.Extensions;
using BotFramework.Previewer.HandlersMetadata.Interfaces;
using BotFramework.Previewer.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BotFramework.Previewer.Extensions;

/// <summary>
/// Представляет методы-расширения для <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Возвращает список типов зарегистрированных обработчиков
    /// </summary>
    /// <param name="services">Контейнер зависимостей</param>
    private static IEnumerable<ServiceDescriptor> GetHandlerDescriptors(this IServiceCollection services)
    {
        return services.Where
        (
            descriptor => descriptor.ServiceType == typeof(IUpdateHandler) && descriptor.ImplementationType is not null
        );
    }

    /// <summary>
    /// Добавляет сериализатор метаданных для анализа в контейнер зависимостей
    /// </summary>
    /// <param name="services">Контейнер зависимостей</param>
    public static IServiceCollection AddMetadataSerializer(this IServiceCollection services)
    {
        services.TryAddSingleton<IMetadataSerializer, MetadataJsonSerializer>();

        return services;
    }
    
    /// <summary>
    /// Добавляет BotFramework Previewer в контейнер зависимостей
    /// </summary>
    /// <param name="services">Контейнер зависимостей</param>
    public static IServiceCollection AddPreviewer(this IServiceCollection services)
    {
        services.AddMetadataSerializer();
        
        var handlersMetadata = services.GetHandlerDescriptors()
            .Select(descriptor => descriptor.ImplementationType)
            .ToAnalysisMetadata();

        services.TryAddTransient<IPreviewerRunner>
        (
            provider => ActivatorUtilities.CreateInstance<PreviewerRunner>(provider, handlersMetadata)
        );

        return services;
    }
}