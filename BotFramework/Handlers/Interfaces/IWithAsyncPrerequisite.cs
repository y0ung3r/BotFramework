using System.Threading.Tasks;

namespace BotFramework.Handlers.Interfaces;

/// <summary>
/// Определяет асинхронное условие выполнение обработчика
/// </summary>
/// <typeparam name="TUpdate">Тип обновления</typeparam>
public interface IWithAsyncPrerequisite<in TUpdate>
	where TUpdate : class
{
	/// <summary>
	/// Проверяет возможно ли выполнить обработку обновления от внешней системы
	/// </summary>
	/// <param name="update">Обновление</param>
	Task<bool> CanHandleAsync(TUpdate update);
}