# BotFramework
![nuget](https://img.shields.io/nuget/dt/BotFramework.NET?label=nuget)

Фреймворк для создания ботов под любую платформу на основе обработки запросов с помощью цепочки обязанностей

## Использование
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
Теперь создайте цепочку обработчиков:
```csharp
var branchBuilder = new BranchBuilder(serviceProvider); // или var branchBuilder = serviceProvider.GetRequiredService<IBranchBuilder>();

branchBuilder.UseHandler<ExceptionHandler>()
             .UseCommand<HelpCommand>()
             .UseCommand<SendCommand>()
             .UseHandler<MissingRequestHandler>();
             
var branch = branchBuilder.Build();
```
*Создайте класс, производный от **BotBase**. Например, для Telegram (свой LongPolling сервис + модели из Telegram.Bot) он будет выглядеть примерно так:
```csharp
public class TelegramBot : BotBase
{
    private readonly ILongPollingService _longPollingService;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public TelegramBot(RequestDelegate rootHandler, ILongPollingService longPollingService) 
        : base(rootHandler)
    {
        _longPollingService = longPollingService;

        _cancellationTokenSource = new CancellationTokenSource();
    }

    public Task<User> GetBotInfoAsync()
    {
        return _client.GetMeAsync();
    }

    public override void Run()
    {
        _longPollingService.Receive
        (
            _rootHandler,
            _cancellationTokenSource.Token
        );
    }

    public override void Stop()
    {
        _cancellationTokenSource.Cancel();
    }
}
```
Передайте цепочку обработчиков боту и запустите его:
```csharp
var botFactory = new BotFactory(serviceProvider); // или var botFactory = serviceProvider.GetRequiredService<IBotFactory>();
var bot = botFactory.Create<TelegramBot>(branch);

bot.Run();

Console.ReadKey();

bot.Stop();
```
**Примечание: можно обойтись и без реализации наследника BotBase*

## Как добавить поддержку новой платформы?
Например, Вы можете реализовать собственный LongPolling сервис, а затем передать в него делегат **RequestDelegate**, полученный из **BranchBuilder**. Когда очередные обновлений от платформы будут получены, вызывайте **RequestDelegate**, передавая в него пакет своих обновлений
