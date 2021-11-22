using System.Threading.Tasks;

namespace BotFramework.Handlers.StepHandlers.Interfaces
{
    /// <summary>
    /// Представляет упакованные на выполнение пошаговые обработчики
    /// </summary>
    internal interface ITransitionContext
    {
        /// <summary>
        /// Указывает запущена ли обработка команды
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Обрабатывает запрос
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="nextHandler">Следующий обработчик по цепочке</param>
        Task HandleAsync(object request, RequestDelegate nextHandler);

        /// <summary>
        /// Проверяет может ли выполняться текущий контекст
        /// </summary>
        /// <param name="request">Запрос</param>
        bool CanHandle(object request);
    }
}