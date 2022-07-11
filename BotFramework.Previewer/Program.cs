using Avalonia;
using Avalonia.ReactiveUI;
using BotFramework.Previewer;

AppBuilder.Configure<App>()
		  .UsePlatformDetect()
		  .LogToTrace()
		  .UseReactiveUI()
		  .StartWithClassicDesktopLifetime(args);