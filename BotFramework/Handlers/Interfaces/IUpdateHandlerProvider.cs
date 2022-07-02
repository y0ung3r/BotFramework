using System.Collections.Generic;

namespace BotFramework.Handlers.Interfaces;

/// <summary>
/// Определяет поставщик обработчиков обновлений
/// </summary>
public interface IUpdateHandlerProvider
{
	/// <summary>
	/// Возвращает доступные обработчики обновлений
	/// </summary>
	/// <typeparam name="TUpdate">Тип обновления</typeparam>
	/// <typeparam name="TClient">Тип внешней системы</typeparam>
    public IEnumerable<IUpdateHandler<TUpdate, TClient>> GetAll<TUpdate, TClient>()
        where TUpdate : class
        where TClient : class;
}