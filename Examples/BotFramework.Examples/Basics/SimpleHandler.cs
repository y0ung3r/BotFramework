using System;
using System.Threading.Tasks;
using Basics.Interfaces;
using BotFramework;
using BotFramework.Handlers.Common;

namespace Basics
{
	/// <summary>
	/// Простой обработчик
	/// </summary>
	public class SimpleHandler : RequestHandlerBase<string>
	{
		private readonly ISimpleService _simpleService;
		
		// Внедрение зависимостей через конструктор
		public SimpleHandler(ISimpleService simpleService)
		{
			_simpleService = simpleService;
		}
		
		public override Task HandleAsync(string request, RequestDelegate nextHandler)
		{
			// Мы можем передать запрос следующему обработчику в цепочке,
			// если по какой-то причине текущий обработчик не сможет его выполнить
			if (request is "Стоп")
			{
				return nextHandler(request);
			}
			
			// Обработка запроса
			Console.WriteLine($"Вы ввели: {request}");

			// Дополнительная обработка с помощью сервиса
			if (_simpleService.BotFrameworkIsCool)
			{
				Console.WriteLine("DI работает!");
			}
			
			return Task.CompletedTask;
		}
	}
}