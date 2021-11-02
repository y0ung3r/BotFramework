using BotFramework.Handlers.Interfaces;

namespace BotFramework.Handlers
{
    /// <summary>
    /// Родительский класс для всех <see cref="ICommandHandler"/>
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса</typeparam>
    public abstract class CommandHandlerBase<TRequest> : RequestHandlerBase<TRequest>, ICommandHandler
        where TRequest : class
    {
        /// <summary>
        /// Проверяет может ли команда быть исполнена
        /// </summary>
        /// <param name="request">Запрос</param>
        public bool CanHandle(object request)
        {
            return request is TRequest typedRequest && CanHandle(typedRequest);
        }

        /// <summary>
        /// Проверяет может ли команда быть исполнена по запросу определенного типа
        /// </summary>
        /// <param name="request">Запрос</param>
        public abstract bool CanHandle(TRequest request);
    }
}
