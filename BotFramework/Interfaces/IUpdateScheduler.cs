using System.Threading.Tasks;

namespace BotFramework.Interfaces;

/// <summary>
/// Определяет планировщик для ожидания новых обновлений от внешней системы
/// </summary>
public interface IUpdateScheduler
{
	/// <summary>
	/// Планирует ожидание для указанного типа обновления
	/// </summary>
	/// <typeparam name="TUpdate">Тип обновления</typeparam>
    Task<TUpdate> ScheduleAsync<TUpdate>();
}