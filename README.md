# BotFramework
[![nuget](https://img.shields.io/nuget/v/BotFramework.NET)](http://www.nuget.org/packages/BotFramework.NET)
[![build](https://img.shields.io/github/workflow/status/y0ung3r/BotFramework/Build)](https://github.com/y0ung3r/BotFramework/actions/workflows/build.yml)
[![downloads](https://img.shields.io/nuget/dt/BotFramework.NET?label=downloads)](http://www.nuget.org/packages/BotFramework.NET)
[![issues](https://img.shields.io/github/issues/y0ung3r/BotFramework)](https://github.com/y0ung3r/BotFramework/issues)
[![pull requests](https://img.shields.io/github/issues-pr/y0ung3r/BotFramework)](https://github.com/y0ung3r/BotFramework/pulls)

Фреймворк для создания ботов под любую платформу

## Возможности
* **Последовательное ожидание обновлений от платформы прямо внутри обработчика;**
* **Запуск из любого места в приложении;**
* **Работа с расширяемыми абстракциями, предназначенные для реализации поддерживаемой логики обработчиков:**
  * Безопасная передача типизированного обновления в обработчик;
  * Проверка пользовательских условий перед обработкой обновления.
* **Поддержка Dependency Injection в обработчиках;**
* **Отображение дерева обработчиков в отдельном приложении.**

## BotFramework Previewer
#### _Особая благодарность [@inyutin-maxim](https://github.com/inyutin-maxim "@inyutin-maxim")_
Начиная с версии **3.1.0**, у Вас имеется возможность просматривать дерево своих обработчиков.
[![build](https://i.imgur.com/97TtQAv.png)](https://i.imgur.com/97TtQAv.png)
#### Установка
Перед использованием Вам нужно установить **Previewer** в качестве инструмента, используя следующую команду терминала:
```shell
dotnet tool install --global BotFramework.NET.Previewer
```
#### Использование
Зарегистрируйте **Previewer** в контейнере зависимостей следующим образом:
```csharp
var services = new ServiceCollection();

// Внедряем BotFramework и обработчики
services.AddBotFramework<ITelegramBotClient>()
        .AddHandler<SimpleHandler>()
        .AddPreviewer();
```
В том месте Вашего кода, где необходимо открыть **Previewer**, используйте **IPreviewerRunner**:
```csharp
var serviceProvider = services.BuildServiceProvider();
var runner = serviceProvider.GetRequiredService<IPreviewerRunner>();
runner.Run();
```
#### Обновление
Следующая команда терминала выполнит обновление инструмента:
```shell
dotnet tool update --global BotFramework.NET.Previewer
```
#### Удаление
Следующая команда терминала выполнит удаление инструмента:
```shell
dotnet tool uninstall --global BotFramework.NET.Previewer
```

## Начало работы
#### Регистрация BotFramework в контейнере зависимостей
Пример регистрации **BotFramework** с использованием **ITelegramBotClient** от [Telegram.Bot](https://github.com/TelegramBots/Telegram.Bot):
```csharp
var services = new ServiceCollection();
			
// Внедряем нужные сервисы как обычно
services.AddTransient<ISimpleService, SimpleService>();
services.AddTransient<ITelegramBotClient>(provider => new TelegramBotClient("TOKEN"))

// Внедряем BotFramework и обработчики
services.AddBotFramework<ITelegramBotClient>()
        .AddHandler<SimpleHandler>();
```

#### Передача обновлений в BotFramework
Для передачи обновлений на обработку используется интерфейс **IUpdateReceiver**. В нем определен единственный метод **Receive(...)**, который выполнит за Вас все необходимое, чтобы доставить обновление к нужному обработчику:
```csharp
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
        // Получение новых сообщений от Telegram
        if (update.Type == UpdateType.Message)
        {
            _receiver.Receive(update.Message);
        }

        return Ok();
    }
}
```
Помните, что **IUpdateReceiver** регистрируется в контейнере зависимостей, как Singleton. Таким образом, Вы можете вызывать метод **Receive(...)** из любой точки приложения.

## Обработчики
#### Реализация обработчика
Типичный обработчик выглядит следующим образом:
```csharp
public class SimpleHandler : IUpdateHandler<string, ITelegramBotClient>
{
    public Task HandleAsync(string update, IBotContext<ITelegramBotClient> context)
    {
        Console.WriteLine($"Получено обновление: {update}");
    }
}
```
Интерфейс **IUpdateHandler** реализуется с использованием типа обновления и типа внешней системы. _В данном случае, ITelegramBotClient выступает в роли внешней системы, а string - тип обрабатываемого обновления_.

#### Внедрение зависимостей в обработчики
Обработчики внедряются в контейнер зависимостей также, как и прочие пользовательские зависимости. Допустим, у нас есть некоторый внешний сервис, выполняющий сложную логику. Вам достаточно внедрить его в контейнер зависимостей и затем получить через конструктор обработчика:
```csharp
public class SimpleHandler : IUpdateHandler<string, ITelegramBotClient>
{
    private readonly ISimpleService _simpleService;

    public SimpleHandler(ISimpleService simpleService)
    {
        _simpleService = simpleService;
    }

    public async Task HandleAsync(string update, IBotContext<ITelegramBotClient> context)
    {
        Console.WriteLine($"Получено обновление: {update}");
        
        await _simpleService.DoWorkAsync(update);
    }
}
```

#### Условные обработчики
Чтобы **BotFramework** выполнял конкретный обработчик по условию, необходимо реализовать для него интерфейс **IWithAsyncPrerequisite**:
```csharp
public class StartHandler : IUpdateHandler<Message, ITelegramBotClient>, IWithAsyncPrerequisite<Message>
{
    public async Task HandleAsync(Message command, IBotContext<ITelegramBotClient> context)
    {
        await context.Client.SendTextMessageAsync(chatId, "Hello world!");
    }
    
    public async Task<bool> CanHandleAsync(Message command)
    {
        return Task.FromResult(command.Text == "/start");
    }
}
```
_**Примечание.** Асинхронный дизайн для условия выбран потому, что зачастую в этой части кода возникает необходимость выполнять асинхронные задачи._

#### Взаимодействие с внешней системой
Для взаимодействия с внешней системой в метод обработчика поставляется интерфейс **IBotContext**. Он содержит в себе ссылку на внешнюю систему и дополнительный метод, позволяющий создать запрос на ожидание будущего обновления. Отличным примером может служить пошаговая обработка ботом сообщений от пользователя:
```csharp
public class SimpleHandler : IUpdateHandler<Message, ITelegramBotClient>
{
    private readonly ISimpleService _simpleService;

    public SimpleHandler(ISimpleService simpleService)
    {
        _simpleService = simpleService;
    }

    public async Task HandleAsync(Message command, IBotContext<ITelegramBotClient> context)
    {
        var chatId = update.Chat.Id;
        await context.Client.SendTextMessageAsync(chatId, "Пожалуйста, введите Вашу фамилию");
        
        var lastname = await context.WaitNextUpdateAsync<Message>();
        await context.Client.SendTextMessageAsync(chatId, "Хорошо, теперь введите Ваше имя");
        
        var firstname = await context.WaitNextUpdateAsync<Message>();
        await context.Client.SendTextMessageAsync(chatId, $"{lastname} {firstname}");
    }
}
```
_**Примечание.** Такой дизайн обработчиков позволяет эффективно сдерживать логику в конкретных местах  и не размазывать ее по всему приложению._
