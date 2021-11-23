using System;
using System.Threading.Tasks;
using BotFramework;
using BotFramework.Attributes;
using BotFramework.Handlers.Common;
using BotFramework.Handlers.Extensions;

namespace Basics
{
	/// <summary>
	/// Простая команда
	/// </summary>
	[CommandAliases("/simple, /command, /example")]
	public class SimpleCommand : CommandHandlerBase<string>
	{
		public override Task HandleAsync(string request, RequestDelegate nextHandler)
		{
			// Обработка команды
			Console.WriteLine("Команда исполнена!");

			return Task.CompletedTask;
		}
		
		// Команда выполнится только если request соответствует одному из псевдонимов, заданных в CommandAliases
		public override bool CanHandle(string request) => this.TextIsCommandAlias(request);
	}
}