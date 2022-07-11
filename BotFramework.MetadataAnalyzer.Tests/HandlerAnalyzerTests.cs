using System.Collections.Generic;
using BotFramework.MetadataAnalyzer.Exceptions;
using BotFramework.MetadataAnalyzer.Tests.Handlers;
using BotFramework.Previewer.HandlersMetadata;
using FluentAssertions;
using Moq.AutoMock;
using Xunit;

namespace BotFramework.MetadataAnalyzer.Tests;

/// <summary>
/// Тесты для <see cref="HandlerAnalyzer"/>
/// </summary>
public class HandlerAnalyzerTests
{
	private readonly AutoMocker _mocker;

	public HandlerAnalyzerTests()
	{
		_mocker = new AutoMocker();
	}

	private HandlerMetadata CreateHandlerMetadata() => new
	(
		typeof(TestHandler)
	);
	
	[Fact]
	public void Should_define_user_handler_type_assembly()
	{
		// Arrange
		var metadata = CreateHandlerMetadata();

		// Act
		var sut = () => new HandlerAnalyzer(metadata);

		// Assert
		sut.Should().NotThrow<HandlerAssemblyNotLoadedException>();
	}

	[Fact]
	public void Should_retrieve_information_about_handler()
	{
		// Arrange
		var metadata = CreateHandlerMetadata();
		var expectedUpdateName = "String";
		var expectedSchedulerCalls = new List<SchedulerCallingDefinition>
		{
			new("String")
		};

		// Act
		var sut = new HandlerAnalyzer(metadata);

		// Assert
		var schedulerCalls = sut.GetSchedulerCalls();
		schedulerCalls.Should().HaveCount(1);
		schedulerCalls.Should().BeEquivalentTo(expectedSchedulerCalls);
		sut.GetUpdateName().Should().Be(expectedUpdateName);
		sut.IsPrerequisiteDefined().Should().BeTrue();
	}
}