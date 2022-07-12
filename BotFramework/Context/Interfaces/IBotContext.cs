using System.Threading.Tasks;

namespace BotFramework.Context.Interfaces;

/// <summary>
/// Определяет контекст для обработчика обновления
/// </summary>
/// <typeparam name="TClient">Тип внешней система</typeparam>
public interface IBotContext<out TClient>
{
    /// <summary>
    /// Внешняя система
    /// </summary>
    public TClient Client { get; }
    
    /// <summary>
    /// Ожидает следующего обновления от внешней системы
    /// </summary>
    /// <typeparam name="TUpdate">Тип обновления</typeparam>
    Task<TUpdate> WaitNextUpdateAsync<TUpdate>()
        where TUpdate: class;
}