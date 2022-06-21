using System.Threading.Tasks;

namespace BotFramework.Interfaces;

public interface IUpdateHandler<in TUpdate>
    where TUpdate: class
{
    Task HandleAsync(TUpdate update, IBotContext context);
}