namespace WebApi
{
    /// <summary>
    /// Конфигурация для Telegram Bot API
    /// </summary>
    public class BotConfiguration
    {
        /// <summary>
        /// Токен
        /// </summary>
        public string Token { get; init; }
        
        /// <summary>
        /// Адрес на который осуществляет поставка обновлений от Telegram
        /// </summary>
        public string WebHookAddress { get; init; }
    }
}