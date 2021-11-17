using System.Threading.Tasks;
using BotFramework.Handlers.StepHandlers.Interfaces;

namespace BotFramework.Handlers.StepHandlers
{
    /// <summary>
    /// Родительский класс для всех <see cref="IStepHandler"/>
    /// </summary>
    /// <typeparam name="TPreviousRequest"></typeparam>
    /// <typeparam name="TCurrentRequest"></typeparam>
    public abstract class StepHandlerBase<TPreviousRequest, TCurrentRequest> : IStepHandler
        where TPreviousRequest: class 
        where TCurrentRequest : class
    {
        /// <summary>
        /// Обработать запрос
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="nextHandler">Следующий обработчик по цепочке</param>
        public Task HandleAsync(object request, RequestDelegate nextHandler)
        {
            return HandleAsync
            (
                default(TPreviousRequest), 
                request
            );
        }

        /// <summary>
        /// Проверяет может ли команда быть исполнена
        /// </summary>
        /// <param name="request">Запрос</param>
        public bool CanHandle(object request)
        {
            return CanHandle
            (
                default(TPreviousRequest), 
                request
            );
        }
        
        /// <summary>
        /// Обрабатывает запрос
        /// </summary>
        /// <param name="previousRequest">Запрос, полученный на предыдущим шаге</param>
        /// <param name="currentRequest">Текущий запрос</param>
        public Task HandleAsync(object previousRequest, object currentRequest)
        {
            return HandleAsync(previousRequest as TPreviousRequest, currentRequest as TCurrentRequest);
        }

        /// <summary>
        /// Проверяет может ли выполниться текущий шаг
        /// </summary>
        /// <param name="previousRequest">Запрос, полученный на предыдущим шаге</param>
        /// <param name="currentRequest">Текущий запрос</param>
        public bool CanHandle(object previousRequest, object currentRequest)
        {
            return previousRequest is TPreviousRequest typedPreviousRequest &&
                   currentRequest is TCurrentRequest typedRightRequest &&
                   CanHandle(typedPreviousRequest, typedRightRequest);
        }

        /// <summary>
        /// Обрабатывает запрос
        /// </summary>
        /// <param name="previousRequest">Запрос, полученный на предыдущим шаге</param>
        /// <param name="currentRequest">Текущий запрос</param>
        public abstract Task HandleAsync(TPreviousRequest previousRequest, TCurrentRequest currentRequest);

        /// <summary>
        /// Проверяет может ли выполниться текущий шаг
        /// </summary>
        /// <param name="previousRequest">Запрос, полученный на предыдущим шаге</param>
        /// <param name="currentRequest">Текущий запрос</param>
        public virtual bool CanHandle(TPreviousRequest previousRequest, TCurrentRequest currentRequest) => true;
    }
}