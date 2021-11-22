using System;
using System.Threading.Tasks;
using BotFramework;
using BotFramework.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace StepHandlers
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

			// Внедряем BotFramework и обработчики
			services.AddBotFramework()
					.AddHandler<Command>()
					.AddHandler<FirstnameHandler>()
					.AddHandler<LastnameHandler>();

			// Создание экземпляра BranchBuilder для построения цепочки
			var branchBuilder = new BranchBuilder(services);
			
			// Конфигурирование BranchBuilder
			branchBuilder.UseStepsFor<Command>
			(
				// Конфигурирование пошагового обработчика
				stepsBuilder =>
				{
					stepsBuilder.UseStepHandler<FirstnameHandler>()
								.UseStepHandler<LastnameHandler>();
				},
				// Так как в примере используется консольное приложение,
				// уникальный ключ может быть любым, однако в реальной ситуации используйте,
				// к примеру, идентификатор чата
				request => nameof(Command) 
			);

			// Получение готовой цепочки
			var branch = branchBuilder.Build();
			
			// Передаем готовую цепочку в любой сервис, который осуществляет получение обновлений от любой платформы
			await RunAsync(branch);
		}
	}
}