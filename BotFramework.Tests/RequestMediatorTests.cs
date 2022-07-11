using BotFramework.Interfaces;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace BotFramework.Tests;

/// <summary>
/// Тесты для <see cref="RequestMediator"/>
/// </summary>
public class RequestMediatorTests
{
	private readonly AutoMocker _mocker;

	public RequestMediatorTests()
	{
		_mocker = new AutoMocker();
	}

	[Fact]
	public void Should_schedule_waiting_next_update()
	{
		// Arrange
		var sut = _mocker.CreateInstance<RequestMediator>();
		
		// Act
		sut.ScheduleAsync<string>();
		
		// Assert
		var updateType = typeof(string);
		sut.Awaiters.Should().Contain(awaiter => awaiter.UpdateType == updateType);
	}
	
	[Fact]
	public void Should_receive_update_and_invoke_handlers()
	{
		// Arrange
		var update = "Payload";
		var sut = _mocker.CreateInstance<RequestMediator>();
		
		// Act
		sut.Receive(update);
		
		// Assert
		_mocker.Verify<IHandlerInvoker>
		(
			invoker => invoker.InvokeAsync(sut, update),
			Times.Once
		);
	}
}