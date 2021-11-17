using BotFramework.Handlers.StepHandlers.Interfaces;
using BotFramework.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BotFramework.Extensions
{
    /// <summary>
    /// Методы-расширения для <see cref="IStepsBuilder"/>
    /// </summary>
    public static class StepsBuilderExtensions
    {
        /// <summary>
        /// Добавляет пошаговый обработчик в цепочку
        /// </summary>
        /// <typeparam name="TStepHandler">Тип пошагового обработчика</typeparam>
        public static IStepsBuilder UseStepHandler<TStepHandler>(this IStepsBuilder builder)
            where TStepHandler : IStepHandler
        {
            var branchBuilder = builder as IBranchBuilder;
            var serviceProvider = branchBuilder.ServiceProvider;
            
            return builder.UseStepHandler
            (
                serviceProvider.GetRequiredService<TStepHandler>()
            );
        }
    }
}