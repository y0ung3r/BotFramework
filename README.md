# BotFramework
![nuget](https://img.shields.io/nuget/v/BotFramework.NET)
![downloads](https://img.shields.io/nuget/dt/BotFramework.NET?label=downloads) 
![issues](https://img.shields.io/github/issues/y0ung3r/BotFramework)
![pull requests](https://img.shields.io/github/issues-pr/y0ung3r/BotFramework)

Фреймворк для создания ботов под любую платформу на основе обработки запросов с помощью цепочки обязанностей

# Использование
*Все примеры ниже оформлены с использованием [Telegram.Bot](https://github.com/TelegramBots/Telegram.Bot)*
### Регистрация зависимостей
Перед началом работы зарегистрируйте зависимости и создайте **ServiceProvider**:
```csharp
var services = new ServiceCollection();

/* Здесь Вы можете внедрить свои зависимости */

services.AddBotFramework()
        .AddHandler<ExceptionHandler>()
        .AddHandler<HelpCommand>()
        .AddHandler<SendCommand>()
        .AddHandler<MissingRequestHandler>();
       
var serviceProvider = services.BuildServiceProvider();
```
Метод **AddBotFramework()** добавляет основные классы фреймворка в контейнер зависимостей. **AddHandler()** регистрирует обработчик запроса указанного типа.

### Конфигурация цепочки обработчиков
Как только все зависимости будут зарегистрированы, используйте **BranchBuilder**, чтобы сконфигурировать цепочку обработчиков:
```csharp
var branchBuilder = new BranchBuilder(serviceProvider); // или var branchBuilder = serviceProvider.GetRequiredService<IBranchBuilder>();

branchBuilder.UseHandler<ExceptionHandler>()
             .UseCommand<HelpCommand>()
             .UseCommand<SendCommand>()
             .UseHandler<MissingRequestHandler>();
```
Метод **UseHandler()** добавляет обработчик в цепочку. **UseCommand()** добавляет команду в обработчик. По сути, команда и есть обработчик, а их отличие в том, что команда вызывается только при соблюдении определенных условий, описанных в реализации этой команды (например, запрос содержит текстовую команду). Также существует метод **UseAnotherBranch()**, которая конфигурирует вложенную цепочку обработчиков. Пример ниже конфигурирует цепочку обработчиков таким образом, чтобы при получении сообщения с текстовой командой и видео, запускался механизм проверки формата ролика в **CheckVideoFormatHandler**, а затем выполнений действий по обработке в **ProcessVideoCommand**:
```csharp
branchBuilder.UseHandler<ExceptionHandler>()
             .UseAnotherBranch
             (
                 request => update.IsVideo() && update.IsCommand(), // Пользовательские методы
                 anotherBranchBuilder => 
                 {
                     anotherBranchBuilder.UseHandler<CheckVideoFormatHandler>()
                                         .UseCommand<ProcessVideoCommand>();
                 }
             )
             .UseCommand<HelpCommand>();
```
Метод **Build()** построит готовую цепочку в виде **RequestDelegate**. Достаточно вызывать этот делегат каждый раз, когда запрос для обработки будет готов будет готов. Например, получение обновления от Telegram:
```csharp
var branch = branchBuilder.Build();
var request = GetLastUpdate(); // Пользовательский метод
await branch(request);
```

### Создание обработчиков
Чтобы создать новый обработчик, реализуйте интерфейс **IRequestHandler**. Пример ниже уведомляет пользователя о том, что бот не может обработать запрос:
```csharp
public class MissingUpdateHandler : IRequestHandler
{
    private readonly ILogger<MissingUpdateHandler> _logger;
    private readonly ITelegramBotClient _client;

    public MissingUpdateHandler(ILogger<MissingUpdateHandler> logger, ITelegramBotClient client)
    {
        _logger = logger;
        _client = client;
    }

    public async Task HandleAsync(object request, RequestDelegate nextHandler)
    {
        var update = request as Update;

        if (update is not null) 
        { 
            _logger.LogWarning($"No handler for request with type: {update.Type}");
            
            if (update.CallbackQuery is CallbackQuery callbackQuery)
            {
                await _client.AnswerCallbackQueryAsync
                (
                    callbackQuery.Id,
                    text: "Невозможно обработать Ваш запрос"
                );
            }
            else if (update.GetChatId() is long chatId && update.GetChatType() is not ChatType.Group) // Пользовательские методы
            {
                await _client.SendTextMessageAsync
                (
                    chatId,
                    text: "Некорректный запрос. Используйте /help для получения списка доступных команд"
                );
            }
        }
    }
}
```
Обратите внимание, что все обработчики поддерживают инъекцию зависимостей.

### Создание команд
Чтобы создать новую команду, используйте интерфейс **ICommandHandler**. Пример ниже по команде "/help" или "/start" выводит пользователю подсказку по работе с ботом:
```csharp
[CommandText("/help, /start")]
public class StartCommand : ICommandHandler
{
    private readonly ILogger<StartCommand> _logger;
    private readonly ITelegramBotClient _client;

    public StartCommand(ILogger<StartCommand> logger, ITelegramBotClient client)
    {
        _logger = logger;
        _client = client;
    }

    public async Task HandleAsync(object request, RequestDelegate nextHandler)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("<b>С помощью данного бота Вы можете: ...</b>");

        var update = request as Update;

        if (update is not null)
        { 
            var message = update.Message;
            var chatId = message.Chat.Id;

            await _client.SendChatActionAsync
            (
                chatId, 
                chatAction: ChatAction.Typing
            );

            await _client.SendTextMessageAsync
            (
                chatId,
                text: stringBuilder.ToString(),
                parseMode: ParseMode.Html,
                disableWebPagePreview: true
            );

            _logger?.LogInformation("Help/Start command processed");
        }
    }

    public bool CanHandle(IServiceProvider serviceProvider, object request)
    {
        var botInfo = _client.GetMeAsync()
                             .GetAwaiter()
                             .GetResult();

        return request is Update update &&
               update.IsCommand() &&
               update.Message is Message message &&
               message.IsContainsBotMention(botInfo) &&
               this.TextIsCommandAlias(message.Text);
    }
}
```
Атрибут **CommandText** позволяет через запятую определить на какие текстовые команды будет реагировать данный обработчик. Метод **CanHandle()** определяет при каких условиях команда может быть выполнена. В примере выше, условие выполнится, если сообщение включает в себя упоминание бота и является текстовой командой, указанной в атрибуте **CommandText**.
Команды также, как и обычные обработчики, поддерживают инъекцию зависимостей. В дополнение к классическому DI, **CanHandle()** содержит в качестве параметра **IServiceProvider**.

### Создание бота
**BotFramework** имеет абстракцию **BotBase** для того, чтобы изолировать вызовы **RequestDelegate** в одном месте. Это вовсе необязательно и вы вольны реализовывать вызовы Вашей цепочки так, как Вам хочется. Пример ниже показывает, как LongPolling-сервис вызывает **RequestDelegate** с именем **rootHandler** при получении новых обновлений от Telegram:
```csharp
public class TelegramBot : BotBase
{
    private readonly ILogger _logger;
    private readonly ILongPollingService _longPollingService;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public TelegramBot(RequestDelegate rootHandler, ILogger<TelegramBot> logger, 
        ILongPollingService longPollingService) : base(rootHandler)
    {
        _logger = logger;
        _longPollingService = longPollingService;

        _cancellationTokenSource = new CancellationTokenSource();
    }

    public override void Run()
    {
        _longPollingService.Receive
        (
            _rootHandler,
            _cancellationTokenSource.Token
        );

        _logger?.LogInformation("Bot is running...");
    }

    public override void Stop()
    {
        _cancellationTokenSource.Cancel();

        _logger?.LogInformation("Bot stopped");
    }
}
```
