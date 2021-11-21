using System.Threading.Tasks;

namespace BotFramework.Handlers.StepHandlers.States
{
	/// <summary>
	/// Родительский класс для всех состояний <see cref="TransitionHandler"/>
	/// </summary>
	internal abstract class TransitionHandlerStateBase
	{
		/// <summary>
		/// Обработчик пошаговых переходов используемый в состоянии
		/// </summary>
		protected readonly TransitionHandler _transitionHandler;
		
		/// <summary>
		/// Базовый конструктор
		/// </summary>
		/// <param name="transitionHandler">Обработчик пошаговых переходов</param>
		protected TransitionHandlerStateBase(TransitionHandler transitionHandler) => _transitionHandler = transitionHandler;

		/// <summary>
		/// Обрабатывает запрос в соответствии с текущим состоянием
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="nextHandler">Следующий в цепочке обработчик запроса</param>
		public abstract Task HandleAsync(object request, RequestDelegate nextHandler);

		/// <summary>
		/// Указывает может ли быть обработан запрос в соответствии с текущим состоянием
		/// </summary>
		/// <param name="request">Запрос</param>
		public abstract bool CanHandle(object request);
	}
}