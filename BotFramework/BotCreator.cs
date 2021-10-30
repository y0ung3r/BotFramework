using BotFramework.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BotFramework
{
    /// <summary>
    /// Стандартная реализация для <see cref="IBotCreator"/>
    /// </summary>
    public class BotCreator : IBotCreator
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="serviceProvider">Поставщик сервисов</param>
        public BotCreator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Создать бота с указанным типом
        /// </summary>
        /// <typeparam name="TBot">Тип бота</typeparam>
        /// <param name="branch">Цепочка обязанностей</param>
        public TBot Create<TBot>(RequestDelegate branch)
            where TBot : IBot
        {
            return ActivatorUtilities.CreateInstance<TBot>(_serviceProvider, branch);
        }
    }
}
