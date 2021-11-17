using System;
using System.Threading.Tasks;
using BotFramework.Handlers.StepHandlers;

namespace BotFramework.Example.First
{
    internal class EndHandler : StepHandlerBase<string, string>
    {
        public override Task HandleAsync(string previousRequest, string currentRequest)
        {
            Console.WriteLine($"Текст с предыдущего шага: {previousRequest}");
            Console.WriteLine($"Вы ввели текст: {currentRequest}");
            Console.WriteLine("Конец команды /first");

            return Task.CompletedTask;
        }
    }
}