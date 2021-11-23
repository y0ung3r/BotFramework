using System.Threading.Tasks;
using BotFramework;
using BotFramework.Extensions;
using BotFramework.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using WebApi.Handlers;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BotController : ControllerBase
    {
        private readonly RequestDelegate _branch;
        
        // Конфигурация BranchBuilder
        public BotController(IBranchBuilder branchBuilder)
        {
            _branch = branchBuilder.UseHandler<EchoHandler>()
                                   .Build();
        }
        
        [HttpPost]
        [Route("GetUpdates")]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            // Отправка новых обновлений от Telegram в цепочку обработчиков
            await _branch(update);

            return Ok();
        }
    }
}