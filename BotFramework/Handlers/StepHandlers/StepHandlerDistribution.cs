using System.Collections.Generic;
using BotFramework.Handlers.Common.Interfaces;
using BotFramework.Handlers.StepHandlers.Interfaces;

namespace BotFramework.Handlers.StepHandlers
{
	/// <summary>
	/// Представляет упакованные на выполнение пошаговые обработчики с уникальным ключом
	/// </summary>
	internal sealed class StepHandlerDistribution
	{
		/// <summary>
		/// Уникальный ключ
		/// </summary>
		public object UniqueKey { get; }
		
		/// <summary>
		/// Выполняемая команда
		/// </summary>
		public ICommandHandler Command { get; }
		
		/// <summary>
		/// Обработчики, которые все еще были не выполнены
		/// </summary>
		public Stack<IStepHandler> RemainingHandlers { get; }
		
		/// <summary>
		/// Запрос, который был обработан последним обработчиком
		/// </summary>
		public object PreviousRequest { get; }
		
		/// <summary>
		/// Следующий на выполнение пошаговый обработчик
		/// </summary>
		public IStepHandler NextStepHandler { get; }
	}
}