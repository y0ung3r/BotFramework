using System.Threading.Tasks;

namespace BotFramework.Interfaces
{
    /// <summary>
    /// Определяет обработчик запроса
    /// </summary>
    public interface IRequestHandler
    {
        /// <summary>
        /// Обработать запрос
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="nextHandler">Следующий обработчик по цепочке</param>
        Task HandleAsync(object request, RequestDelegate nextHandler);
    }
}