using System.Threading.Tasks;

namespace BotFramework.Interfaces;

/// <summary>
/// Определяет механизм запуска обработчиков обновлений
/// </summary>
public interface IHandlerInvoker
{
	/// <summary>
	/// Перенаправляет обновление доступным обработчикам
	/// </summary>
	/// <param name="scheduler">Планировщик для ожидания новых обновлений от внешней системы</param>
	/// <param name="update">Обновление</param>
	/// <typeparam name="TUpdate">Тип обновления</typeparam>
	Task InvokeAsync<TUpdate>(IUpdateScheduler scheduler, TUpdate update);
}