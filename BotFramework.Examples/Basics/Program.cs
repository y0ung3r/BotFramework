using System;
using Basics.Interfaces;
using BotFramework.Extensions;
using BotFramework.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Basics;

public class Program
{
	public static void Main(string[] args)
	{
		// Регистрация зависимостей
		var services = new ServiceCollection();
			
		// Внедряем нужные сервисы как обычно
		services.AddTransient<ISimpleService, SimpleService>();

		// Внедряем BotFramework и обработчики
		services.AddBotFramework(_ => Console.Out)
				.AddHandler<SimpleHandler>();

		var serviceProvider = services.BuildServiceProvider();
			
		// Получение UpdateReceiver, который будет принимать запросы от внешней системы
		var receiver = serviceProvider.GetRequiredService<IUpdateReceiver>();
			
		while (true)
		{
			// Получаем запрос от внешней системы
			var request = Console.ReadLine();
			receiver.Receive(request);
		}
	}
}