using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace WebApi
{
    /// <summary>
    /// См. https://github.com/TelegramBots/Telegram.Bot.Examples/blob/master/Telegram.Bot.Examples.WebHook/Services/ConfigureWebhook.cs
    /// </summary>
    public class WebhookConfiguration : IHostedService
    {
        private readonly BotConfiguration _botConfiguration;
        private readonly ITelegramBotClient _client;

        public WebhookConfiguration(IConfiguration configuration, ITelegramBotClient client)
        {
            _botConfiguration = configuration.GetSection("BotConfiguration")
                                             .Get<BotConfiguration>();

            _client = client;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var webhookAddress = @$"{_botConfiguration.WebHookAddress}/Bot/GetUpdates";
            
            await _client.SetWebhookAsync
            (
                url: webhookAddress,
                allowedUpdates: Array.Empty<UpdateType>(),
                cancellationToken: cancellationToken
            );
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _client.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }
}