using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
            var fakeCommand = new FakeCommand();
            var fakeStepHandler = new FakeStepHandler();
            var fakeStepHandlers = new ReadOnlyCollection<IStepHandler>
            (
                new List<IStepHandler>()
                {
                    fakeStepHandler
                }
            );

            _mocker.Use<ICommandHandler>(fakeCommand);
            _mocker.Use<IStepHandler>(fakeStepHandler);
            _mocker.Use<IReadOnlyCollection<IStepHandler>>(fakeStepHandlers);

            _mocker.Setup<ITransitionContext>
            (
                context => context.HandleAsync
                (
                    It.IsAny<object>(),
                    It.IsAny<RequestDelegate>()
                )
            );

            return _mocker.CreateInstance<TransitionHandler>();
        }

        [Test]
        public async Task Successfully_processing_of_the_request()
        {
            // Arrange
            var sut = CreateTransitionHandler();
            
            // Act
            await sut.HandleAsync
            (
                "/fake", 
                It.IsAny<RequestDelegate>()
            );
            
            // Assert
            _mocker.Verify<ITransitionContext>
            (
                context => context.HandleAsync
                (
                    It.IsAny<object>(),
                    It.IsAny<RequestDelegate>()
                ),
                Times.Once()
            );
        }

        [TearDown]
        public void TearDown()
        {
            _mocker.AsDisposable()
                   .Dispose();
        }
    }
}