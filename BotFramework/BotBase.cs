using BotFramework.Interfaces;

namespace BotFramework
{
    /// <summary>
    /// Представляет базовую реализацию <see cref="IBot"/>
    /// </summary>
    public abstract class BotBase : IBot
    {
        protected readonly RequestDelegate _rootHandler;

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="rootHandler">Первый обработчик цепочки</param>
        public BotBase(RequestDelegate rootHandler)
        {
            _rootHandler = rootHandler;
        }

        public abstract void Run();

        public abstract void Stop();
    }
}
