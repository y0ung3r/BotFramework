using System.Threading.Tasks;
using BotFramework.Context.Interfaces;

namespace BotFramework.Handlers.Interfaces;

/// <summary>
/// Определяет обработчик обновления
/// </summary>
public interface IUpdateHandler
{ }

/// <summary>
/// Определяет обработчик обновления
/// </summary>
/// <typeparam name="TUpdate">Тип обновления</typeparam>
/// <typeparam name="TClient">Тип внешней системы</typeparam>
public interface IUpdateHandler<in TUpdate, in TClient> : IUpdateHandler
{
    /// <summary>
    /// Выполняет обработку обновления, используя указанный контекст
    /// </summary>
    /// <param name="update">Обновление от внешней системы</param>
    /// <param name="context">Контекст</param>
    Task HandleAsync(TUpdate update, IBotContext<TClient> context);
}