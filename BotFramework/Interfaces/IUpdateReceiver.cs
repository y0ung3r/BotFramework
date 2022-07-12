using BotFramework.Extensions;

namespace BotFramework.Interfaces;

/// <summary>
/// Определяет точку получения обновлений от внешней системы
/// </summary>
public interface IUpdateReceiver
{
	/// <summary>
	/// Передает обновление от внешней системы на обработку
	/// </summary>
	/// <param name="update">Обновление</param>
	void Receive(object update);
}