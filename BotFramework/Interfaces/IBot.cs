namespace BotFramework.Interfaces
{
    /// <summary>
    /// Определяет бота
    /// </summary>
    public interface IBot
    {
        /// <summary>
        /// Запустить бота
        /// </summary>
        void Run();

        /// <summary>
        /// Остановить бота
        /// </summary>
        void Stop();
    }
}
