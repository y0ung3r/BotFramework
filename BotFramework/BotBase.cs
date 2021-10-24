using BotFramework.Interfaces;

namespace BotFramework
{
    /// <summary>
    /// Представляет базовую реализацию <see cref="IBot"/>
    /// </summary>
    public abstract class BotBase : IBot
    {
        /// <summary>
        /// Обработчики запросов, прикрепленные к текущему боту
        /// </summary>
        protected readonly RequestDelegate _rootHandler;

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="rootHandler">Первый обработчик цепочки</param>
        public BotBase(RequestDelegate rootHandler)
        {
            _rootHandler = rootHandler;
        }

        /// <summary>
        /// Запустить бота
        /// </summary>
        public abstract void Run();

        /// <summary>
        /// Остановить бота
        /// </summary>
        public abstract void Stop();
    }
}
