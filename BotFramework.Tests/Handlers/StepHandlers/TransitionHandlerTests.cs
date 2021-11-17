using System.Collections.Generic;
using System.Collections.ObjectModel;
using BotFramework.Handlers.Common.Interfaces;
using BotFramework.Handlers.StepHandlers;
using BotFramework.Handlers.StepHandlers.Interfaces;
using BotFramework.Tests.Fakes;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;

namespace BotFramework.Tests.Handlers.StepHandlers
{
    /// <summary>
    /// Тесты для <see cref="TransitionHandler"/>
    /// </summary>
    public class TransitionHandlerTests
    {
        private readonly AutoMocker _mocker;
        
        /// <summary>
        /// Базовый конструктор
        /// </summary>
        public TransitionHandlerTests()
        {
            _mocker = new AutoMocker();
        }
        
        private TransitionHandler CreateTransitionHandler()
        {
            var fakeCommand = new FakeCommandHandler();
            var fakeStepHandler = new FakeStepHandler();
            var fakeStepHandlers = new ReadOnlyCollection<IStepHandler>
            (
                new List<IStepHandler>()
                {
                    fakeStepHandler
                }
            );

            _mocker.Use<ICommandHandler>(fakeCommand);
            _mocker.Setup<ICommandHandler>
            (
                command => command.HandleAsync
                (
                    It.IsAny<object>(),
                    It.IsAny<RequestDelegate>()
                )
            );

            _mocker.Use<IStepHandler>(fakeStepHandler);
            _mocker.Setup<IStepHandler>
            (
                stepHandler => stepHandler.HandleAsync
                (
                    It.IsAny<object>(),
                    It.IsAny<object>()
                )
            );

            _mocker.Use<IReadOnlyCollection<IStepHandler>>(fakeStepHandlers);
            
            return _mocker.CreateInstance<TransitionHandler>();
        }

        [Test]
        public void Transition_handler_was_running()
        {
            // Arrange
            var sut = CreateTransitionHandler();
            
            // Act
            sut.HandleAsync
            (
                "/fake", 
                It.IsAny<RequestDelegate>()
            );
            
            // Assert
            _mocker.Verify<ICommandHandler>
            (
                command => command.HandleAsync
                (
                    It.IsAny<object>(),
                    It.IsAny<RequestDelegate>()
                ),
                Times.Once()
            );

            sut.IsRunning
               .Should()
               .BeTrue();
        }

        [TearDown]
        public void TearDown()
        {
            _mocker.AsDisposable()
                   .Dispose();
        }
    }
}