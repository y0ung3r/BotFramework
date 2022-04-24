using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotFramework.Handlers.Common.Interfaces;
using BotFramework.Handlers.StepHandlers.Interfaces;
using Microsoft.Extensions.Logging;

namespace BotFramework.Handlers.StepHandlers
{
	/// <summary>
	/// Стандартная реализация для <see cref="TransitionContext"/>
	/// </summary>
	internal sealed class TransitionContext : ITransitionContext
	{
		private readonly ILogger<TransitionContext> _logger;
		private readonly ICommandHandler _command;
		private readonly IReadOnlyCollection<IStepHandler> _source;
		private readonly Stack<IStepHandler> _remainingHandlers;
		private object _previousRequest;

		/// <summary>
		/// Указывает запущена ли обработка команды
		/// </summary>
		public bool IsRunning => _remainingHandlers.Any();

		/// <summary>
		/// Базовый конструктор
		/// </summary>
		/// <param name="logger">Сервис логгирования</param>
		/// <param name="command">Команда, выполняемая пошаговыми обработчиками</param>
		/// <param name="source">Пошаговые обработчики</param>
		public TransitionContext(ILogger<TransitionContext> logger, ICommandHandler command, IReadOnlyCollection<IStepHandler> source)
		{
			_logger = logger;
			_command = command;
			_source = source;

			_remainingHandlers = new Stack<IStepHandler>();
		}

		/// <summary>
		/// Показывает может ли выполниться следующий пошаговый обработчик
		/// </summary>
		/// <param name="request">Запрос</param>
		private bool NextStepHandlerCanHandle(object request) => IsRunning && _remainingHandlers.Peek().CanHandle(_previousRequest, request);

		/// <summary>
		/// Показывает может ли выполниться команда, которая перезапустит выполнение пошаговых обработчиков
		/// </summary>
		/// <param name="request">Запрос</param>
		private bool CommandCanHandle(object request) => _command.CanHandle(request);

		/// <summary>
		/// Обрабатывает команду
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="nextHandler">Следующий обработчик по цепочке</param>
		private Task InvokeCommandAsync(object request, RequestDelegate nextHandler)
		{
			if (_logger.IsEnabled(LogLevel.Information))
			{
				_logger.LogInformation("Запрос передан на выполнение обработчику команды");
			}

			if (_remainingHandlers.Count > 0)
			{
				_remainingHandlers.Clear();
			}
            
			foreach (var handler in _source)
			{
				_remainingHandlers.Push(handler);
			}

			return _command.HandleAsync(request, nextHandler);
		}

		/// <summary>
		/// Обрабатывает запрос, используя следующий на выполнение пошаговый обработчик
		/// </summary>
		/// <param name="request">Запрос</param>
		private Task InvokeStepHandlerAsync(object request)
		{
			if (_logger.IsEnabled(LogLevel.Information))
			{
				_logger.LogInformation("Запрос передан следующему на выполнение пошаговому обработчику");
			}

			return _remainingHandlers.Pop().HandleAsync(_previousRequest, request);
		}

		/// <summary>
		/// Обрабатывает запрос
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="nextHandler">Следующий обработчик по цепочке</param>
		public async Task HandleAsync(object request, RequestDelegate nextHandler)
		{
			if (CommandCanHandle(request) && !NextStepHandlerCanHandle(request))
			{
				await InvokeCommandAsync(request, nextHandler).ConfigureAwait(false);
			}
			else
			{
				await InvokeStepHandlerAsync(request).ConfigureAwait(false);
			}

			_previousRequest = request;
		}

		/// <summary>
		/// Проверяет может ли выполняться текущий контекст
		/// </summary>
		/// <param name="request">Запрос</param>
		public bool CanHandle(object request) => CommandCanHandle(request) || NextStepHandlerCanHandle(request);
	}
}