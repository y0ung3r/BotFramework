using BotFramework.Handlers.Interfaces;
using BotFramework.Handlers.StepHandler;
using BotFramework.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace BotFramework.Extensions
{
    /// <summary>
    /// Методы-расширения для <see cref="IBranchBuilder"/>
    /// </summary>
    public static class BranchBuilderExtensions
    {
        /// <summary>
        /// Добавляет в цепочку обработчик запроса по заданному <see cref="IRequestHandler"/> и возвращает текущий экземпляр построителя цепочки обязанностей
        /// </summary>
        /// <typeparam name="TRequestHandler">Обработчик запроса, который необходимо добавить в цепочку</typeparam>
        /// <param name="builder">Построитель цепочки обязанностей</param>
        /// <returns>Текущий экземпляр построителя цепочки обязанностей</returns>
        public static IBranchBuilder UseHandler<TRequestHandler>(this IBranchBuilder builder)
            where TRequestHandler : IRequestHandler
        {
            return builder.UseHandler
            (
                builder.ServiceProvider.GetRequiredService<TRequestHandler>()
            );
        }

        /// <summary>
        /// Добавляет в цепочку отдельную ветвь, а внутрь него помещает заданный обработчик запроса <see cref="IRequestHandler"/> и возвращает текущий экземпляр построителя цепочки обязанностей
        /// </summary>
        /// <typeparam name="TRequestHandler">Обработчик запроса, который необходимо добавить в цепочку</typeparam>
        /// <param name="builder">Построитель цепочки обязанностей</param>
        /// <param name="predicate">Условие, при котором происходит переход к добавляемой ветви при обработке запроса</param>
        /// <returns>Текущий экземпляр построителя цепочки обязанностей</returns>
        public static IBranchBuilder UseAnotherBranch<TRequestHandler>(this IBranchBuilder builder, Predicate<object> predicate)
            where TRequestHandler : IRequestHandler
        {
            return builder.UseAnotherBranch
            (
                predicate,
                branchBuilder => branchBuilder.UseHandler<TRequestHandler>()
            );
        }

        /// <summary>
        /// Добавляет в цепочку обработчик команды и возвращает текущий экземпляр построителя цепочки обязанностей
        /// </summary>
        /// <param name="builder">Построитель цепочки обязанностей</param>
        /// <param name="commandHandler">Обработчик команды, который необходимо добавить в цепочку</param>
        /// <returns>Текущий экземпляр построителя цепочки обязанностей</returns>
        public static IBranchBuilder UseCommand(this IBranchBuilder builder, ICommandHandler commandHandler)
        {
            return builder.UseAnotherBranch
            (
                commandHandler.CanHandle,
                branchBuilder => branchBuilder.UseHandler(commandHandler)
            );
        }

        /// <summary>
        /// Добавляет в цепочку обработчик команды по заданному <see cref="ICommandHandler"/> и возвращает текущий экземпляр построителя цепочки обязанностей
        /// </summary>
        /// <typeparam name="TCommandHandler">Обработчик команды, который необходимо добавить в цепочку</typeparam>
        /// <param name="builder">Построитель цепочки обязанностей</param>
        /// <returns>Текущий экземпляр построителя цепочки обязанностей</returns>
        public static IBranchBuilder UseCommand<TCommandHandler>(this IBranchBuilder builder)
            where TCommandHandler : ICommandHandler
        {
            return builder.UseCommand
            (
                builder.ServiceProvider.GetRequiredService<TCommandHandler>()
            );
        }

        /// <summary>
        /// Добавляет пошаговый обработчик в цепочку обработчиков
        /// </summary>
        /// <param name="builder">Построитель цепочки обязанностей</param>
        /// <param name="commandHandler">Обработчик команды, который должен пошагового обрабатываться</param>
        /// <param name="configure">Конфигурация</param>
        /// <returns>Текущий экземпляр построителя цепочки обязанностей</returns>
        public static IBranchBuilder UseStepHandler(this IBranchBuilder builder, ICommandHandler commandHandler, Action<IBranchBuilder> configure)
        {
            var anotherBranchBuilder = builder.ServiceProvider.GetRequiredService<IBranchBuilder>();
            configure(anotherBranchBuilder);
            
            var transitionHandlerFactory = builder.ServiceProvider.GetRequiredService<Func<IReadOnlyCollection<IRequestHandler>, ICommandHandler, TransitionHandler>>();

            return builder.UseCommand
            (
                transitionHandlerFactory(anotherBranchBuilder.Handlers, commandHandler)
            );
        }

        /// <summary>
        /// Добавляет пошаговый обработчик с указанным типом в цепочку обработчиков
        /// </summary>
        /// <typeparam name="TCommandHandler">Тип обработчик команды, который должен пошагового обрабатываться</typeparam>
        /// <param name="builder">Построитель цепочки обязанностей</param>
        /// <param name="configure">Конфигурация</param>
        /// <returns>Текущий экземпляр построителя цепочки обязанностей</returns>
        public static IBranchBuilder UseStepHandler<TCommandHandler>(this IBranchBuilder builder, Action<IBranchBuilder> configure)
            where TCommandHandler : ICommandHandler
        {
            return builder.UseStepHandler
            (
                builder.ServiceProvider.GetRequiredService<TCommandHandler>(),
                configure
            );
        }
    }
}
