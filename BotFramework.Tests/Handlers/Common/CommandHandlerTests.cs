using System.Collections.Generic;
using System.Linq;
using BotFramework.Extensions;
using BotFramework.Tests.Fakes;
using FluentAssertions;
using NUnit.Framework;

namespace BotFramework.Tests.Handlers.Common
{
    /// <summary>
    /// Тесты для <see cref="CommandHandlerExtensions"/>
    /// </summary>
    public class CommandHandlerTests
    {
        [Test]
        public void Successfully_getting_the_command_attribute()
        {
            // Arrange
            var sut = new FakeCommandHandler();

            // Act
            var commandAttribute = sut.GetCommandAttribute();

            // Assert
            commandAttribute.Should()
                            .NotBeNull();
        }

        [Test]
        public void Unsuccessfully_getting_the_command_attribute()
        {
            // Arrange
            var sut = new FakeCommandHandlerWithoutAliases();

            // Act
            var commandAttribute = sut.GetCommandAttribute();

            // Assert
            commandAttribute.Should()
                            .BeNull();
        }

        [Test]
        public void Successfully_getting_aliases()
        {
            // Arrange
            var sut = new FakeCommandHandler();

            // Act
            var commandAttribute = sut.GetCommandAliases();

            // Assert
            commandAttribute.Should()
                            .BeEquivalentTo(new List<string>
                            {
                                "/fake",
                                "/command"
                            });
        }

        [Test]
        public void Unsuccessfully_getting_aliases()
        {
            // Arrange
            var sut = new FakeCommandHandlerWithoutAliases();

            // Act
            var commandAttribute = sut.GetCommandAliases();

            // Assert
            commandAttribute.Should()
                            .BeEquivalentTo
                            (
                                Enumerable.Empty<string>()
                            );
        }

        [Test]
        public void Test_is_empty()
        {
            // Arrange
            var sut = new FakeCommandHandler();
            
            // Act
            var isCommandAlias = sut.TextIsCommandAlias(string.Empty);

            // Assert
            isCommandAlias.Should()
                          .BeFalse();
        }
        
        [Test]
        public void Text_is_command_alias()
        {
            // Arrange
            var sut = new FakeCommandHandler();

            // Act
            var isCommandAlias = sut.TextIsCommandAlias("/command");

            // Assert
            isCommandAlias.Should()
                          .BeTrue();
        }

        [Test]
        public void Text_is_not_command_alias()
        {
            // Arrange
            var sut = new FakeCommandHandlerWithoutAliases();

            // Act
            var isCommandAlias = sut.TextIsCommandAlias("/fake");

            // Assert
            isCommandAlias.Should()
                          .BeFalse();
        }
    }
}
