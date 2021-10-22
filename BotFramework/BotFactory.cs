using BotFramework.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BotFramework
{
    public class BotFactory : IBotFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public BotFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TBot Create<TBot>(RequestDelegate branch)
            where TBot : IBot
        {
            return ActivatorUtilities.CreateInstance<TBot>(_serviceProvider, branch);
        }
    }
}
