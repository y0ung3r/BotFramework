using BotFramework.Extensions;
using BotFramework.Interfaces;
using BotFramework.Tests.Fakes;
using FluentAssertions;
using Moq.AutoMock;
using NUnit.Framework;

namespace BotFramework.Tests
{
    public class BranchBuilderTests
    {
        private readonly AutoMocker _mocker;
        private IBranchBuilder _sut;

        public BranchBuilderTests()
        {
            _mocker = new AutoMocker();
        }

        [SetUp]
        public void Setup()
        {
            _sut = _mocker.CreateInstance<IBranchBuilder>();
        }

        [Test]
        public void Building_a_branch_with_a_handler()
        {
            // Arrange
            _sut.UseHandler<FakeRequestHandler>();

            // Act
            var requestDelegate = _sut.Build();

            // Assert
            requestDelegate.Should().NotBeNull();
        }

        [Test]
        public void Building_a_branch_with_a_command()
        {
            // Arrange
            _sut.UseCommand<FakeCommandHandler>();

            // Act
            var requestDelegate = _sut.Build();

            // Assert
            requestDelegate.Should().NotBeNull();
        }

        [Test]
        public void Building_a_branch_with_a_handler_and_a_command()
        {
            // Arrange
            _sut.UseHandler<FakeRequestHandler>()
                .UseCommand<FakeCommandHandler>();

            // Act
            var requestDelegate = _sut.Build();

            // Assert
            requestDelegate.Should().NotBeNull();
        }

        [TearDown]
        public void TearDown()
        {
            _sut = null;
        }
    }
}
