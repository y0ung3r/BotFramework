using System.Threading.Tasks;

namespace BotFramework.Interfaces;

public interface IHandlerInvoker
{
	Task InvokeAsync<TUpdate>(IUpdateScheduler scheduler, TUpdate update)
		where TUpdate : class;
}