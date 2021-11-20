# BotFramework
[![nuget](https://img.shields.io/nuget/v/BotFramework.NET)](http://www.nuget.org/packages/BotFramework.NET)
[![downloads](https://img.shields.io/nuget/dt/BotFramework.NET?label=downloads)](http://www.nuget.org/packages/BotFramework.NET)
[![issues](https://img.shields.io/github/issues/y0ung3r/BotFramework)](https://github.com/y0ung3r/BotFramework/issues)
[![pull requests](https://img.shields.io/github/issues-pr/y0ung3r/BotFramework)](https://github.com/y0ung3r/BotFramework/pulls)

Фреймворк для создания ботов под любую платформу на основе обработки запросов с помощью цепочки обязанностей

## Возможности
* **Построение цепочек обработчиков под любые задачи:**
  * Простые обработчики;
  * Команды, выполняемые по условию;
  * Вложенные цепочки простых обработчиков и команд, выполняемые по условию;
  * Обработчики для пошаговой обработки команд.
* **Запуск цепочки обработчиков из любого места в приложении;**
* **Работа с расширяемыми абстракциями, предназначенные для реализации поддерживаемой логики обработчиков:**
  * Создавайте собственные обертки над обработчиками;
  * Указывайте типы запросов с помощью обобщений.
* **Поддержка псевдонимов для обработчиков команд:**
  * Определяйте любое количество текстовых команд (псевдонимов) для своих обработчиков;
  * Используйте методы-расширения из **BotFramework** для проверки соответствия между введенной пользователем командой и заданным Вами списком псевдонимов.
* **Совместимость с `Microsoft.Extensions.Logging` и `Microsoft.Extensions.DependencyInjection`**;
* **Поддержка Dependency Injection в обработчиках**.

## Начало работы
*Все примеры ниже оформлены с использованием [Telegram.Bot](https://github.com/TelegramBots/Telegram.Bot)*
### Регистрация зависимостей
Перед началом работы зарегистрируйте зависимости и создайте `ServiceProvider`:
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
Метод `AddBotFramework()` добавляет основные классы фреймворка в контейнер зависимостей. `AddHandler()` регистрирует обработчик запроса указанного типа.

### Конфигурация цепочки обработчиков
Как только все зависимости будут зарегистрированы, используйте `BranchBuilder`, чтобы сконфигурировать цепочку обработчиков:
```csharp
var branchBuilder = new BranchBuilder(serviceProvider); // или var branchBuilder = serviceProvider.GetRequiredService<IBranchBuilder>();

branchBuilder.UseHandler<ExceptionHandler>()
             .UseCommand<HelpCommand>()
             .UseCommand<SendCommand>()
             .UseStepsFor<GreetingCommand>(stepsBuilder => 
             {
                 stepsBuilder.UseStepHandler<FirstnameHandler>()
                             .UseStepHandler<LastnameHandler>();
             })
             .UseHandler<MissingRequestHandler>();
```
Метод `UseHandler()` добавляет обработчик в цепочку. `UseCommand()` добавляет команду в обработчик. По сути, команда и есть обработчик, а их отличие в том, что команда вызывается только при соблюдении определенных условий, описанных в реализации этой команды (например, запрос содержит текстовую команду). `UseStepsFor()` позволяет сконфигурировать и добавить пошаговый обработчик для команды. Также существует метод `UseAnotherBranch()`, которая конфигурирует вложенную цепочку обработчиков. Пример ниже конфигурирует цепочку обработчиков таким образом, чтобы при получении сообщения с текстовой командой и видео, запускался механизм проверки формата ролика в `CheckVideoFormatHandler`, а затем выполнений действий по обработке в `ProcessVideoHandler`:
```csharp
branchBuilder.UseHandler<ExceptionHandler>()
             .UseAnotherBranch
             (
                 request => update.IsVideo() && update.IsCommand(), // Пользовательские методы
                 anotherBranchBuilder => 
                 {
                     anotherBranchBuilder.UseHandler<CheckVideoFormatHandler>()
                                         .UseHandler<ProcessVideoHandler>();
                 }
             )
             .UseCommand<HelpCommand>();
```
Метод `Build()` построит готовую цепочку в виде `RequestDelegate`. Достаточно вызывать этот делегат каждый раз, когда запрос для обработки будет готов. Например, получение обновления от Telegram:
```csharp
var branch = branchBuilder.Build();
var request = GetLastUpdate(); // Пользовательский метод
await branch(request);
```