using System.Threading.Tasks;

namespace BotFramework.Abstractions
{
    /// <summary>
    /// Родительский класс для всех <see cref="IRequestHandler"/>
    /// </summary>
    /// <typeparam name="TRequest">Тип запроса</typeparam>
    public abstract class RequestHandlerBase<TRequest> : IRequestHandler
        where TRequest : class
    {
        /// <summary>
        /// Обработать запрос
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="nextHandler">Следующий обработчик по цепочке</param>
        public Task HandleAsync(object request, RequestDelegate nextHandler)
        {
            if (request is TRequest typedRequest)
            {
                return HandleAsync(typedRequest, nextHandler);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Обработать запрос с указанным типом
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="nextHandler">Следующий обработчик по цепочке</param>
        public abstract Task HandleAsync(TRequest request, RequestDelegate nextHandler);
    }
}
