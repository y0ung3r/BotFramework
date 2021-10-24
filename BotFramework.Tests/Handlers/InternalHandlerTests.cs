using BotFramework.Extensions;
using BotFramework.Handlers;
using BotFramework.Handlers.Interfaces;
using BotFramework.Tests.Fakes;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using System;

namespace BotFramework.Tests.Handlers
{
    /// <summary>
    /// Тесты для <see cref="InternalHandler" />
    /// </summary>
    public class InternalHandlerTests
    {
        private readonly AutoMocker _mocker;

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        public InternalHandlerTests()
        {
            _mocker = new AutoMocker();
        }

        private InternalHandler CreateInternalHandler<TRequestHandler>(Predicate<object> predicate)
            where TRequestHandler : IRequestHandler
        {
            var requestHandler = Activator.CreateInstance<TRequestHandler>();
            var requestDelegate = new RequestDelegate
            (
                requestHandler.ToRequestDelegate
                (
                    It.IsAny<RequestDelegate>()
                )
            );

            _mocker.Use(requestDelegate);
            _mocker.Use(predicate);

            _mocker.Setup<RequestDelegate>
            (
                requestDelegate => requestDelegate
                (
                    It.IsAny<object>()
                )
            );

            return _mocker.CreateInstance<InternalHandler>();
        }

        [Test]
        public void Successfully_processing_of_the_request()
        {
            // Arrange
            var sut = CreateInternalHandler<FakeRequestHandler>(request => true);

            // Act
            sut.HandleAsync
            (
                request: It.IsAny<object>(), 
                nextHandler: It.IsAny<RequestDelegate>()
            );

            // Assert
            _mocker.Verify<RequestDelegate>
            (
                requestDelegate => requestDelegate
                (
                    It.IsAny<object>()
                ), 
                Times.Once()
            );
        }

        [Test]
        public void Successfully_processing_of_the_command()
        {
            // Arrange
            var sut = CreateInternalHandler<FakeCommandHandler>(request => true);

            // Act
            sut.HandleAsync
            (
                request: It.IsAny<object>(),
                nextHandler: It.IsAny<RequestDelegate>()
            );

            // Assert
            _mocker.Verify<RequestDelegate>
            (
                requestDelegate => requestDelegate
                (
                    It.IsAny<object>()
                ),
                Times.Once()
            );
        }

        [Test]
        public void Predicate_returns_false()
        {
            // Arrange
            var sut = CreateInternalHandler<FakeCommandHandler>(request => false);

            // Act
            sut.HandleAsync
            (
                request: It.IsAny<object>(),
                nextHandler: It.IsAny<RequestDelegate>()
            );

            // Assert
            _mocker.Verify<RequestDelegate>
            (
                requestDelegate => requestDelegate
                (
                    It.IsAny<object>()
                ),
                Times.Never()
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
