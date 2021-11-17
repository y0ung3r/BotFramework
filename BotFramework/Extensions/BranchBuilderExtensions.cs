using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using BotFramework.Handlers.Common;
using BotFramework.Handlers.Common.Interfaces;
using BotFramework.Handlers.StepHandlers;
using BotFramework.Handlers.StepHandlers.Interfaces;
using BotFramework.Interfaces;

namespace BotFramework.Extensions
{
    /// <summary>
    /// Методы-расширения для <see cref="IBranchBuilder"/>
    /// </summary>
    public static class BranchBuilderExtensions
    {
        /// <summary>
        /// Создает и конфигирирует вложенную ветвь обработчиков
        /// </summary>
        /// <param name="builder">Текущая цепочка обработчиков</param>
        /// <param name="configure">Конфигурация для добавляемой ветви</param>
        internal static IBranchBuilder CreateAnotherBranch(this IBranchBuilder builder, Action<IBranchBuilder> configure)
        {
            var anotherBranchBuilder = builder.ServiceProvider.GetRequiredService<IBranchBuilder>();
            
            configure(anotherBranchBuilder);

            return anotherBranchBuilder;
        }
        
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
        /// Добавляет в цепочку отдельную ветвь и возвращает текущий экземпляр построителя цепочки обязанностей
        /// </summary>
        /// <param name="builder">Построитель цепочки обязанностей</param>
        /// <param name="predicate">Условие, при котором происходит переход к добавляемой ветви при обработке запроса</param>
        /// <param name="configure">Конфигурация для добавляемой ветви</param>
        /// <returns>Текущий экземпляр построителя цепочки обязанностей</returns>
        public static IBranchBuilder UseAnotherBranch(this IBranchBuilder builder, Predicate<object> predicate, Action<IBranchBuilder> configure)
        {
            var serviceProvider = builder.ServiceProvider;
            var anotherBranchBuilder = builder.CreateAnotherBranch(configure);
            var anotherBranch = anotherBranchBuilder.Build();
            var internalHandlerFactory = serviceProvider.GetRequiredService<Func<RequestDelegate, Predicate<object>, InternalHandler>>();

            return builder.UseHandler
            (
                internalHandlerFactory(anotherBranch, predicate)
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
        /// Добавляет пошаговые обработчики для команды в цепочку обработчиков
        /// </summary>
        /// <param name="builder">Построитель цепочки обязанностей</param>
        /// <param name="commandHandler">Обработчик команды, который должен пошагового обрабатываться</param>
        /// <param name="configure">Конфигурация</param>
        /// <returns>Текущий экземпляр построителя цепочки обязанностей</returns>
        public static IBranchBuilder UseStepsFor(this IBranchBuilder builder, ICommandHandler commandHandler, Action<IStepsBuilder> configure)
        {
            var serviceProvider = builder.ServiceProvider;
            var anotherBranchBuilder = builder.CreateAnotherBranch
            (
                branchBuilder => configure(branchBuilder as IStepsBuilder)
            );
            
            var transitionHandlerFactory = serviceProvider.GetRequiredService<Func<ICommandHandler, IReadOnlyCollection<IStepHandler>, TransitionHandler>>();
            var stepHandlers = anotherBranchBuilder.Handlers
                                                   .Cast<IStepHandler>()
                                                   .ToList()
                                                   .AsReadOnly();

            return builder.UseCommand
            (
                transitionHandlerFactory(commandHandler, stepHandlers)
            );
        }

        /// <summary>
        /// Добавляет пошаговые обработчики для команды с указанным типом в цепочку обработчиков
        /// </summary>
        /// <typeparam name="TCommandHandler">Тип обработчик команды, который должен пошагового обрабатываться</typeparam>
        /// <param name="builder">Построитель цепочки обязанностей</param>
        /// <param name="configure">Конфигурация</param>
        /// <returns>Текущий экземпляр построителя цепочки обязанностей</returns>
        public static IBranchBuilder UseStepsFor<TCommandHandler>(this IBranchBuilder builder, Action<IStepsBuilder> configure)
            where TCommandHandler : ICommandHandler
        {
            return builder.UseStepsFor
            (
                builder.ServiceProvider.GetRequiredService<TCommandHandler>(),
                configure
            );
        }
    }
}
