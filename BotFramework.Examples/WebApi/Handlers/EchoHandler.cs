using System.Threading.Tasks;
using BotFramework;
using BotFramework.Handlers.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace WebApi.Handlers
{
    // Обработчик, который повторяет текст, введенный пользователем 
    public class EchoHandler : RequestHandlerBase<Update>
    {
        private readonly ITelegramBotClient _client;

        // Внедрение зависимостей для работы с Telegram API
        // См. https://github.com/TelegramBots
        public EchoHandler(ITelegramBotClient client)
        {
            _client = client;
        }
        
        public override async Task HandleAsync(Update request, RequestDelegate nextHandler)
        {
            // Логика текущего обработчика
            if (request.Message is Message message)
            {
                var chatId = message.Chat.Id;
                await _client.SendChatActionAsync(chatId, ChatAction.Typing);
                await _client.SendTextMessageAsync(chatId, $"Вы ввели: **{message.Text}**", ParseMode.Markdown);
            }
            // else ignore
        }
    }
}