using System;
using System.Threading.Tasks;
using BotFramework;
using BotFramework.Attributes;
using BotFramework.Handlers.Common;
using BotFramework.Handlers.Extensions;

namespace StepHandlers
{
	/// <summary>
	/// Команда, которая запустит процесс последовательной обработки имени и фамилии
	/// </summary>
	[CommandAliases("/askMyName")]
	public class Command : CommandHandlerBase<string>
	{
		public override Task HandleAsync(string request, RequestDelegate nextHandler)
		{
			Console.WriteLine("Привет! Я задам пару вопросов...");
			Console.WriteLine("Как Вас зовут?");
			
			return Task.CompletedTask;
		}

		public override bool CanHandle(string request) => this.TextIsCommandAlias(request);
	}
}