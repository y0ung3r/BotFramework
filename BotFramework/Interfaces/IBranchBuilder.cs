using BotFramework.Handlers.Interfaces;
using System;

namespace BotFramework.Interfaces
{
    /// <summary>
    /// Определяет построитель цепочку обязанностей для бота
    /// </summary>
    public interface IBranchBuilder
    {
        /// <summary>
        /// Поставщик сервисов
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Добавляет в цепочку обработчик запроса и возвращает текущий экземпляр построителя цепочки обязанностей
        /// </summary>
        /// <param name="handler">Обработчик запроса, который необходимо добавить в цепочку</param>
        /// <returns>Текущий экземпляр построителя цепочки обязанностей</returns>
        IBranchBuilder UseHandler(IRequestHandler handler);

        /// <summary>
        /// Добавляет в цепочку отдельную ветвь и возвращает текущий экземпляр построителя цепочки обязанностей
        /// </summary>
        /// <param name="predicate">Условие, при котором происходит переход к добавляемой ветви при обработке запроса</param>
        /// <param name="configure">Конфигурация добавляемой ветви</param>
        /// <returns>Текущий экземпляр построителя цепочки обязанностей</returns>
        IBranchBuilder UseAnotherBranch(Predicate<object> predicate, Action<IBranchBuilder> configure);

        /// <summary>
        /// Строит цепочку обязанностей
        /// </summary>
        /// <returns>Цепочка обязанностей</returns>
        RequestDelegate Build();
    }
}
