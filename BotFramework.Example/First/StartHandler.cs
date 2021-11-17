using System;
using System.Threading.Tasks;
using BotFramework.Handlers.StepHandlers;

namespace BotFramework.Example.First
{
    internal class StartHandler : StepHandlerBase<string, string>
    {
        public override Task HandleAsync(string previousRequest, string currentRequest)
        {
            Console.WriteLine($"Текст с предыдущего шага: {previousRequest}");
            Console.WriteLine($"Вы ввели текст: {currentRequest}");
            
            return Task.CompletedTask;
        }
    }
}