using BotFramework.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using BotFramework.Abstractions;
using BotFramework.Example.Common;
using BotFramework.Example.First;
using BotFramework.Example.Second;

namespace BotFramework.Example
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddBotFramework()
                    .AddHandler<MissingRequestHandler>()
                    .AddHandler<SecondCommand>()
                    .AddHandler<FirstCommand>()
                    .AddHandler<StartHandler>()
                    .AddHandler<EndHandler>();

            var serviceProvider = services.BuildServiceProvider();
            var branchBuilder = serviceProvider.GetRequiredService<IBranchBuilder>();

            branchBuilder.UseCommand<SecondCommand>()
                         .UseStepHandler<FirstCommand>(stepHandlerBuilder =>
                         {
                             stepHandlerBuilder.UseHandler<StartHandler>()
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
