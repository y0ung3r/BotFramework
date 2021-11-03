using BotFramework.Handlers.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BotFramework.Handlers
{
    /// <summary>
    /// Представляет переход запроса в указанный обработчик
    /// </summary>
    internal class TransitionHandler : IRequestHandler
    {
        private readonly ILogger<TransitionHandler> _logger;
        private readonly RequestDelegate _from;
        private readonly RequestDelegate _to;

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="logger">Сервис логгирования</param>
        /// <param name="from">Обработчик из которого нужно осуществить переход</param>
        /// <param name="to">Обработчик в который нужно осуществить переход</param>
        public TransitionHandler(ILogger<TransitionHandler> logger, RequestDelegate from, RequestDelegate to)
        {
            _logger = logger;
            _from = from;
            _to = to;
        }

        /// <summary>
        /// Обработать запрос
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="nextHandler">Следующий обработчик по цепочке</param>
        public Task HandleAsync(object request, RequestDelegate nextHandler)
        {
            throw new System.NotImplementedException();
        }
    }
}
