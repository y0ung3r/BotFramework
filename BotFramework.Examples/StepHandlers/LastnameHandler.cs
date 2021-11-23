using System;
using System.Threading.Tasks;
using BotFramework.Handlers.StepHandlers;

namespace StepHandlers
{
	/// <summary>
	/// Обработчик фамилии
	/// </summary>
	public class LastnameHandler : StepHandlerBase<string, string>
	{
		public override Task HandleAsync(string previousRequest, string currentRequest)
		{
			Console.WriteLine($"Я запомнил тебя, {currentRequest} {previousRequest}.");
			Console.WriteLine("Спасибо!");
			
			return Task.CompletedTask;
		}
	}
}