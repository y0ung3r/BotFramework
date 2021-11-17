using System.Threading.Tasks;
using BotFramework.Handlers.Common.Interfaces;

namespace BotFramework.Handlers.StepHandlers.Interfaces
{
    /// <summary>
    /// Определяет пошаговый обработчик
    /// </summary>
    public interface IStepHandler : ICommandHandler
    {
        /// <summary>
        /// Обрабатывает запрос
        /// </summary>
        /// <param name="previousRequest">Запрос, полученный на предыдущим шаге</param>
        /// <param name="currentRequest">Текущий запрос</param>
        Task HandleAsync(object previousRequest, object currentRequest);

        /// <summary>
        /// Проверяет может ли выполниться текущий шаг
        /// </summary>
        /// <param name="previousRequest">Запрос, полученный на предыдущим шаге</param>
        /// <param name="currentRequest">Текущий запрос</param>
        bool CanHandle(object previousRequest, object currentRequest);
    }
}