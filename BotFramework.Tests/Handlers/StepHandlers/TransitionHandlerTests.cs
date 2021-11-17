using Moq.AutoMock;

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
    }
}
