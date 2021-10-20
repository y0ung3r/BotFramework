using System.Threading.Tasks;

namespace BotFramework.Handlers.Interfaces
{
    public interface IRequestHandler
    {
        Task HandleAsync(object request, RequestDelegate nextHandler);
    }
}