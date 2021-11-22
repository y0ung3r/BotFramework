using System;
using System.Threading.Tasks;
using BotFramework.Handlers.StepHandlers;

namespace StepHandlers
{
	/// <summary>
	/// Обработчик имени
	/// </summary>
	public class FirstnameHandler : StepHandlerBase<string, string>
	{
		public override Task HandleAsync(string previousRequest, string currentRequest)
		{
			Console.WriteLine($"Отлично, {previousRequest} {currentRequest}!");
			Console.WriteLine("Теперь, укажите фамилию:");
			
			return Task.CompletedTask;
		}
	}
}