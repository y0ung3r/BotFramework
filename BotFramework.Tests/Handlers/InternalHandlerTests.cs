using BotFramework.Handlers;
using Moq.AutoMock;
using NUnit.Framework;

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

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Processing_of_the_request()
        {
            // Arrange

            // Act

            // Assert

        }

        [Test]
        public void Processing_of_the_command()
        {
            // Arrange

            // Act

            // Assert

        }

        [TearDown]
        public void TearDown()
        {
            _mocker.AsDisposable()
                   .Dispose();
        }
    }
}
