using BotFramework.Extensions;
using BotFramework.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace BotFramework.Example
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddBotFramework()
                    .AddHandler<MissingRequestHandler>()
                    .AddHandler<AnotherCommand>()
                    .AddHandler<BindCommand>()
                    .AddHandler<MiddlewareHandler>()
                    .AddHandler<EndHandler>();

            var serviceProvider = services.BuildServiceProvider();
            var branchBuilder = serviceProvider.GetRequiredService<IBranchBuilder>();

            branchBuilder.UseCommand<AnotherCommand>()
                         .UseStepHandler<BindCommand>(stepHandlerBuilder =>
                         {
                             stepHandlerBuilder.UseHandler<MiddlewareHandler>()
                                               .UseHandler<EndHandler>();
                         })
                         .UseHandler<MissingRequestHandler>();

            var branch = branchBuilder.Build();

            while (true)
            {
                Console.Write(">>> ");
                var request = Console.ReadLine();

                await branch(request);
            }
        }
    }
}
