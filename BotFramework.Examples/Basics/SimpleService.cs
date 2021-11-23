using Basics.Interfaces;

namespace Basics
{
	/// <summary>
	/// Реализация для <see cref="ISimpleService" />
	/// </summary>
	public class SimpleService : ISimpleService
	{
		public bool BotFrameworkIsCool { get; }

		public SimpleService()
		{
			BotFrameworkIsCool = true;
		}
	}
}