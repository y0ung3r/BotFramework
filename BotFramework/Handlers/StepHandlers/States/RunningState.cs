using System.Threading.Tasks;

namespace BotFramework.Handlers.StepHandlers.States
{
	/// <summary>
	/// Определяет запущенное состояние для <see cref="TransitionHandler"/>
	/// </summary>
	internal sealed class RunningState : TransitionHandlerStateBase
	{
		/// <summary>
		/// Базовый конструктор
		/// </summary>
		/// <param name="transitionHandler">Обработчик пошаговых переходов</param>
		public RunningState(TransitionHandler transitionHandler) : base(transitionHandler)
		{ }

		/// <summary>
		/// Обрабатывает запрос в соответствии c запущенным состоянием
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="nextHandler">Следующий в цепочке обработчик запроса</param>
		public override Task HandleAsync(object request, RequestDelegate nextHandler)
		{
			if (!_transitionHandler.CommandCanHandle(request))
			{
				return _transitionHandler.HandleByNextStepHandlerAsync(request);
			}

			_transitionHandler.SetState
			(
				new IdleState(_transitionHandler)
			);

			return _transitionHandler.HandleAsync(request, nextHandler);

		}

		/// <summary>
		/// Указывает может ли быть обработан запрос в соответствии c запущенным состоянием
		/// </summary>
		/// <param name="request">Запрос</param>
		public override bool CanHandle(object request)
		{
			return _transitionHandler.NextStepHandlerCanHandle(request) || _transitionHandler.CommandCanHandle(request);
		}
	}
}