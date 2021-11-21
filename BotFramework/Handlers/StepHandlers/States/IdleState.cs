using System.Threading.Tasks;

namespace BotFramework.Handlers.StepHandlers.States
{
	/// <summary>
	/// Определяет состояние простоя для <see cref="TransitionHandler"/>
	/// </summary>
	internal sealed class IdleState : TransitionHandlerStateBase
	{
		/// <summary>
		/// Базовый конструктор
		/// </summary>
		/// <param name="transitionHandler">Обработчик пошаговых переходов</param>
		public IdleState(TransitionHandler transitionHandler) 
			: base(transitionHandler)
		{ }

		/// <summary>
		/// Обрабатывает запрос в соответствии со состоянием простоя
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="nextHandler">Следующий в цепочке обработчик запроса</param>
		public override Task HandleAsync(object request, RequestDelegate nextHandler)
		{
			_transitionHandler.SetState
			(
				new RunningState(_transitionHandler)
			);
			
			return _transitionHandler.HandleByCommandAsync(request, nextHandler);
		}

		/// <summary>
		/// Указывает может ли быть обработан запрос в соответствии со состоянием простоя
		/// </summary>
		/// <param name="request">Запрос</param>
		public override bool CanHandle(object request)
		{
			return !_transitionHandler.IsRunning && _transitionHandler.CommandCanHandle(request);
		}
	}
}