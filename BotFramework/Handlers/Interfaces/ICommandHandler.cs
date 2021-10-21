using System;

namespace BotFramework.Handlers.Interfaces
{
    /// <summary>
    /// Определяет обработчик команды
    /// </summary>
    public interface ICommandHandler : IRequestHandler
    {
        /// <summary>
        /// Проверяет может ли команда быть исполнена
        /// </summary>
        /// <param name="serviceProvider">Поставщик сервисов</param>
        /// <param name="request">Запрос</param>
        bool CanHandle(IServiceProvider serviceProvider, object request);
    }
}