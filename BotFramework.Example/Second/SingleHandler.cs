using System;
using System.Threading.Tasks;
using BotFramework.Handlers.StepHandlers;

namespace BotFramework.Example.Second
{
    public class SingleHandler : StepHandlerBase<string, string>
    {
        public override Task HandleAsync(string previousRequest, string currentRequest)
        {
            Console.WriteLine("TEST");
            
            return Task.CompletedTask;
        }
    }
}