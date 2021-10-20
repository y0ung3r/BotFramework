using System;

namespace BotFramework.Handlers.Interfaces
{
    public interface ICommandHandler : IRequestHandler
    {
        bool CanHandle(IServiceProvider serviceProvider, object request);
    }
}