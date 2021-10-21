using System.Threading.Tasks;

namespace BotFramework
{
    /// <summary>
    /// Делегат, представляющий собой запрос к цепочке обработчиков
    /// </summary>
    /// <param name="request">Запрос</param>
    public delegate Task RequestDelegate(object request);
}
