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
                    .AddHandler<FakeCommandHandler>();

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
        public void Building_a_branch_with_the_handler()
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
        public void Building_a_branch_with_the_command()
        {
            // Arrange
            var commandHandler = new FakeCommandHandler();

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
        public void Building_a_branch_with_the_handler_and_the_command()
        {
            // Arrange
            var requestHandler = new FakeRequestHandler();
            var commandHandler = new FakeCommandHandler();

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
        public void Building_a_branch_with_the_step_handlers()
        {
            // Arrange
            var fakeCommand = new FakeCommandHandler();
            var fakeStepHandler = new FakeStepHandler();

            // Act
            _sut.UseStepsFor(fakeCommand, stepsBuilder =>
            {
                stepsBuilder.UseStepHandler(fakeStepHandler);
            });
            
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
