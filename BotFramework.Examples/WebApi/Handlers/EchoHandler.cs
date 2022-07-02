using System.Threading.Tasks;
using BotFramework.Context.Interfaces;
using BotFramework.Handlers.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace WebApi.Handlers;

// Обработчик, который повторяет текст, введенный пользователем 
public class EchoHandler : IUpdateHandler<Message, ITelegramBotClient>
{
    public async Task HandleAsync(Message update, IBotContext<ITelegramBotClient> context)
    {
        var chatId = update.Chat.Id;
        await context.Client.SendChatActionAsync(chatId, ChatAction.Typing);
        await context.Client.SendTextMessageAsync(chatId, $"Вы ввели: **{update.Text}**", ParseMode.Markdown);
    }
}