using System.Threading.Tasks;

namespace BotFramework.Handlers.Interfaces;

public interface IWithAsyncPrerequisite<in TUpdate>
	where TUpdate : class
{
	Task<bool> CanHandleAsync(TUpdate update);
}