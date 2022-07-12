using BotFramework.Interfaces;

namespace BotFramework.Context.Interfaces;

/// <summary>
/// Определяет фабрику для создания контекста для обработчика обновления
/// </summary>
/// <typeparam name="TClient">Тип внешней системы</typeparam>
public interface IBotContextFactory<out TClient>
{
    /// <summary>
    /// Создает контекст для обработчика обновления
    /// </summary>
    /// <param name="scheduler">Планировщик для ожидания новых обновлений от внешней системы</param>
    IBotContext<TClient> Create(IUpdateScheduler scheduler);
}