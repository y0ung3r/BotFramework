﻿using BotFramework.Extensions;
using BotFramework.Tests.Fakes;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace BotFramework.Tests.Handlers
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