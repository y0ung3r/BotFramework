using System.Text;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using BotFramework.Previewer.Components.DesignTime;
using BotFramework.Previewer.Extensions;
using BotFramework.Previewer.HandlersMetadata;
using BotFramework.Previewer.HandlersMetadata.Interfaces;
using BotFramework.Previewer.ViewModels;
using BotFramework.Previewer.Views;
using Microsoft.Extensions.DependencyInjection;

namespace BotFramework.Previewer;

public partial class App : Application
{
	public IServiceProvider ServiceProvider { get; private set; }
	
	public IMetadataSerializer MetadataSerializer => ServiceProvider.GetRequiredService<IMetadataSerializer>();
	
	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			var metadata = default(AnalysisMetadata);
			
			#if DEBUG
			metadata = new AnalysisMetadata()
			{
				Handlers = new List<HandlerMetadata>()
				{
					new HandlerMetadata
					(
						typeof(DesignTimeHandler)
					),
					new HandlerMetadata
					(
						typeof(DesignTimeCommand)
					)
				}
			};
			#else
			var startupArgument = Environment.GetCommandLineArgs().Last();
			var encodedMetadata = Convert.FromBase64String(startupArgument);
			var decodedMetadata = Encoding.UTF8.GetString(encodedMetadata);
			metadata = MetadataSerializer.Deserialize(decodedMetadata);
			#endif
				
			desktop.MainWindow = new MainWindow
			{
				DataContext = new MainWindowViewModel(metadata)
			};
		}

		base.OnFrameworkInitializationCompleted();
	}

	public override void RegisterServices()
	{
		ServiceProvider = new ServiceCollection()
			.AddMetadataSerializer()
			.BuildServiceProvider();
		
		base.RegisterServices();
	}
}