using System.Threading.Tasks;
using BotFramework.Context.Interfaces;

namespace BotFramework.Handlers.Interfaces;

/// <summary>
/// Определяет асинхронное условие выполнение обработчика
/// </summary>
/// <typeparam name="TUpdate">Тип обновления</typeparam>
/// <typeparam name="TClient">Тип внешней системы</typeparam>
public interface IWithAsyncPrerequisite<in TUpdate, in TClient>
{
	/// <summary>
	/// Проверяет возможно ли выполнить обработку обновления от внешней системы
	/// </summary>
	/// <param name="update">Обновление</param>
	/// <param name="context">Контекст</param>
	Task<bool> CanHandleAsync(TUpdate update, IBotContext<TClient> context);
}