using BotFramework.Extensions;
using BotFramework.Tests.Fakes;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Linq;
using BotFramework.Handlers.Common;

namespace BotFramework.Tests
{
    /// <summary>
    /// Тесты для <see cref="BranchBuilder"/>
    /// </summary>
    public class BranchBuilderTests
    {
        private BranchBuilder _sut;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();

            services.AddBotFramework()
                    .AddHandler<FakeRequestHandler>()
                    .AddHandler<FakeCommand>();

            _sut = new BranchBuilder(services);
        }

        [Test]
        public void Building_a_branch()
        {
            // Arrange
            var fakeHandler = new FakeRequestHandler();
            
            // Act
            var branch = _sut.UseHandler(fakeHandler)
                             .Build();
            
            // Assert
            branch.Should()
                  .NotBeNull();
        }

        [Test]
        public void Injecting_a_handler_in_the_branch()
        {
            // Arrange
            var requestHandler = new FakeRequestHandler();

            // Act
            _sut.UseHandler(requestHandler);

            // Assert
            var handlers = _sut.Handlers;

            handlers.Should()
                    .HaveCount(1);

            handlers.Should()
                    .ContainEquivalentOf(requestHandler);
        }

        [Test]
        public void Injecting_a_command_in_the_branch()
        {
            // Arrange
            var commandHandler = new FakeCommand();

            // Act
            _sut.UseCommand(commandHandler);

            // Assert
            var handlers = _sut.Handlers;

            handlers.Should()
                    .HaveCount(1);

            handlers.ElementAt(index: 0)
                    .Should()
                    .BeOfType<InternalHandler>();
        }

        [Test]
        public void Injecting_a_handler_and_a_command_in_the_branch()
        {
            // Arrange
            var requestHandler = new FakeRequestHandler();
            var commandHandler = new FakeCommand();

            // Act
            _sut.UseHandler(requestHandler)
                .UseCommand(commandHandler);

            // Assert
            var handlers = _sut.Handlers;

            handlers.Should()
                    .HaveCount(2);

            handlers.ElementAt(index: 0)
                    .Should()
                    .BeOfType<InternalHandler>();

            handlers.ElementAt(index: 1)
                    .Should()
                    .BeEquivalentTo(requestHandler);
        }

        [Test]
        public void Injecting_a_command_step_handlers_in_the_branch()
        {
            // Arrange
            var fakeCommand = new FakeCommand();
            var fakeStepHandler = new FakeStepHandler();

            // Act
            _sut.UseStepsFor
            (
	            fakeCommand, 
	            stepsBuilder =>
	            {
	                stepsBuilder.UseStepHandler(fakeStepHandler);
	            },
	            _ => nameof(FakeCommand)
	        );
            
            // Assert
            _sut.Handlers.ElementAt(index: 0)
                         .Should()
                         .BeOfType<InternalHandler>();
        }

        [TearDown]
        public void TearDown()
        {
            _sut = null;
        }
    }
}
