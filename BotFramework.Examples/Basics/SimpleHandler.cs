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

		public async Task HandleAsync(string update, IBotContext<TextWriter> context)
		{
			Console.WriteLine($"Обновление получено: {update}");
			
			// Ожидание обновлений от внешней системы
			Console.WriteLine("Введите дополнительные данные:");
			var payload = await context.WaitNextUpdateAsync<string>();
			Console.WriteLine(payload);
			
			// Дополнительная логика с использованием сервиса
			if (_simpleService.BotFrameworkIsCool)
			{
				Console.WriteLine("DI работает!");
			}
		}
	}
}