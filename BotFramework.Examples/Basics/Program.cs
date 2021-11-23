using System;
using System.Threading.Tasks;
using Basics.Interfaces;
using BotFramework;
using BotFramework.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Basics
{
	public class Program
	{
		private static async Task RunAsync(RequestDelegate branch)
		{
			// Получаем и передаем пакет обновлений нашей цепочке обработчиков
			while (true)
			{
				Console.Write(">>> ");
				
				var request = Console.ReadLine();
				await branch(request);
			}
		}
		
		public static async Task Main(string[] args)
		{
			// Регистрация зависимостей
			var services = new ServiceCollection();
			
			// Внедряем нужные сервисы как обычно
			services.AddTransient<ISimpleService, SimpleService>();

			// Внедряем BotFramework и обработчики
			services.AddBotFramework()
					.AddHandler<SimpleCommand>()
					.AddHandler<SimpleHandler>();

			// Создание экземпляра BranchBuilder для построения цепочки
			var branchBuilder = new BranchBuilder(services);
			
			// Конфигурирование BranchBuilder
			branchBuilder.UseCommand<SimpleCommand>()
				         .UseHandler<SimpleHandler>();

			// Получение готовой цепочки
			var branch = branchBuilder.Build();
			
			// Передаем готовую цепочку в любой сервис, который осуществляет получение обновлений от любой платформы
			await RunAsync(branch);
		}
	}
}