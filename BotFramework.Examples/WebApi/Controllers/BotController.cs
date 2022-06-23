using BotFramework.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BotController : ControllerBase
{
    private readonly IUpdateReceiver _receiver;
    
    public BotController(IUpdateReceiver receiver)
    {
        _receiver = receiver;
    }
        
    [HttpPost]
    [Route("GetUpdates")]
    public IActionResult Post([FromBody] Update update)
    {
        // Отправка новых сообщений от Telegram в цепочку обработчиков
        if (update.Type == UpdateType.Message)
        {
            _receiver.Receive(update.Message);
        }

        return Ok();
    }
}