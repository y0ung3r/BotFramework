using BotFramework.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using BotFramework.Example.Common;
using BotFramework.Example.First;
using BotFramework.Example.Second;
using BotFramework.Interfaces;

namespace BotFramework.Example
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddBotFramework()
                    .AddHandler<MissingRequestHandler>()
                    .AddHandler<FirstCommand>()
                    .AddHandler<StartHandler>()
                    .AddHandler<EndHandler>()
                    .AddHandler<SecondCommand>()
                    .AddHandler<SingleHandler>();

            var serviceProvider = services.BuildServiceProvider();
            var branchBuilder = serviceProvider.GetRequiredService<IBranchBuilder>();

            branchBuilder.UseStepsFor<FirstCommand>(stepsBuilder =>
                         {
                             stepsBuilder.UseStepHandler<StartHandler>()
                                         .UseStepHandler<EndHandler>();
                         })
                         .UseStepsFor<SecondCommand>(stepsBuilder =>
                         {
                             stepsBuilder.UseStepHandler<SingleHandler>();
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
