using Microsoft.Extensions.DependencyInjection;
using Previewer;

var services = new ServiceCollection();

// Внедряем BotFramework и обработчики
services.AddBotFramework(_ => Console.Out)
		.AddHandler<SimpleHandler>()
		.AddPreviewer();

var serviceProvider = services.BuildServiceProvider();
			
// Получение UpdateReceiver, который будет принимать запросы от внешней системы
var runner = serviceProvider.GetRequiredService<IPreviewerRunner>();
runner.Run();