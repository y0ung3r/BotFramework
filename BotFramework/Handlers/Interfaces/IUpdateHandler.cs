using System.Threading.Tasks;
using BotFramework.Context.Interfaces;

namespace BotFramework.Handlers.Interfaces;

public interface IUpdateHandler
{ }

public interface IUpdateHandler<in TUpdate, TClient> : IUpdateHandler
    where TUpdate : class
    where TClient : class
{
    Task HandleAsync(TUpdate update, IBotContext<TClient> context);
}