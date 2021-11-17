namespace BotFramework.Handlers.StepHandlers.Interfaces
{
    /// <summary>
    /// Определяет построитель пошаговых обработчиков
    /// </summary>
    public interface IStepsBuilder
    {
        /// <summary>
        /// Добавляет пошаговый обработчик в цепочку
        /// </summary>
        /// <param name="handler">Пошаговый обработчик</param>
        IStepsBuilder UseStepHandler(IStepHandler handler);
    }
}