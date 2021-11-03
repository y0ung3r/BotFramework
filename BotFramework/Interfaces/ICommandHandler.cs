namespace BotFramework.Interfaces
{
    /// <summary>
    /// Определяет обработчик команды
    /// </summary>
    public interface ICommandHandler : IRequestHandler
    {
        /// <summary>
        /// Проверяет может ли команда быть исполнена
        /// </summary>
        /// <param name="request">Запрос</param>
        bool CanHandle(object request);
    }
}