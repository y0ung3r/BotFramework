using System;
using System.IO;
using System.Threading.Tasks;
using Basics.Interfaces;
using BotFramework.Context.Interfaces;
using BotFramework.Handlers.Interfaces;

namespace Basics
{
	/// <summary>
	/// Простой обработчик
	/// </summary>
	public class SimpleHandler : IUpdateHandler<string, TextWriter>
	{
		private readonly ISimpleService _simpleService;
		
		// Внедрение зависимостей через конструктор
		public SimpleHandler(ISimpleService simpleService)
		{
			_simpleService = simpleService;
		}

		public Task HandleAsync(string update, IBotContext<TextWriter> context)
		{
			Console.WriteLine($"Обновление получено: {update}");
			
			// Дополнительная логика с помощью сервиса
			if (_simpleService.BotFrameworkIsCool)
			{
				Console.WriteLine("DI работает!");
			}
			
			return Task.CompletedTask;
		}
	}
}