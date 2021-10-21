using BotFramework.Extensions;
using BotFramework.Handlers;
using BotFramework.Handlers.Interfaces;
using BotFramework.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BotFramework
{
    public class BranchBuilder : IBranchBuilder
    {
        private readonly Stack<IRequestHandler> _handlers;

        public IServiceProvider ServiceProvider { get; }

        public IReadOnlyCollection<IRequestHandler> Handlers => _handlers.ToList().AsReadOnly();

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="serviceProvider">Поставщик сервисов</param>
        public BranchBuilder(IServiceProvider serviceProvider)
        {
            _handlers = new Stack<IRequestHandler>();
            
            ServiceProvider = serviceProvider;
        }

        public IBranchBuilder UseHandler(IRequestHandler handler)
        {
            _handlers.Push(handler);

            return this;
        }

        public IBranchBuilder UseAnotherBranch(Predicate<object> predicate, Action<IBranchBuilder> configure)
        {
            var anotherBranchBuilder = ServiceProvider.GetRequiredService<IBranchBuilder>();
            configure(anotherBranchBuilder);

            var anotherBranch = anotherBranchBuilder.Build();
            var internalHandlerFactory = ServiceProvider.GetRequiredService<Func<RequestDelegate, Predicate<object>, InternalHandler>>();

            return UseHandler
            (
                internalHandlerFactory(anotherBranch, predicate)
            );
        }

        public RequestDelegate Build() 
        {
            var rootHandler = default(RequestDelegate);

            var branch = _handlers.Select
            (
                handler => new Func<RequestDelegate, RequestDelegate>
                (
                    next => handler.ToRequestDelegate(next)
                )
            );

            foreach (var handler in branch)
            {
                rootHandler = handler(rootHandler);
            }

            return rootHandler;
        }
    }
}
