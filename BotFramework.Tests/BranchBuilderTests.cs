﻿using BotFramework.Extensions;
using BotFramework.Tests.Fakes;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Linq;

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
        public void Building_a_branch_with_the_handler()
        {
            // Arrange
            var requestHandler = new FakeRequestHandler();

            // Act
            var requestDelegate = _sut.UseHandler(requestHandler)
                                      .Build();

            // Assert
            var handlers = _sut.Handlers;

            handlers.Should()
                    .HaveCount(1);

            handlers.Should()
                    .ContainEquivalentOf(requestHandler);

            requestDelegate.Should()
                           .NotBeNull();
        }

        [Test]
        public void Building_a_branch_with_the_command()
        {
            // Arrange
            var commandHandler = new FakeCommandHandler();

            // Act
            var requestDelegate = _sut.UseCommand(commandHandler)
                                      .Build();

            // Assert
            var handlers = _sut.Handlers;

            handlers.Should()
                    .HaveCount(1);

            handlers.ElementAt(index: 0)
                    .Should()
                    .BeOfType<InternalHandler>();

            requestDelegate.Should()
                           .NotBeNull();
        }

        [Test]
        public void Building_a_branch_with_the_handler_and_the_command()
        {
            // Arrange
            var requestHandler = new FakeRequestHandler();
            var commandHandler = new FakeCommandHandler();

            // Act
            var requestDelegate = _sut.UseHandler(requestHandler)
                                      .UseCommand(commandHandler)
                                      .Build();

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

            requestDelegate.Should()
                           .NotBeNull();
        }

        [TearDown]
        public void TearDown()
        {
            _sut = null;
        }
    }
}
