using AvaloniaGraphControl;
using BotFramework.MetadataAnalyzer;
using BotFramework.Previewer.Components;
using BotFramework.Previewer.HandlersMetadata;

namespace BotFramework.Previewer.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
	public Graph HandlersGraph { get; }

	public MainWindowViewModel(AnalysisMetadata metadata)
	{
		ArgumentNullException.ThrowIfNull(metadata);

		HandlersGraph = CreateGraph(metadata);
	}

	private HandlersGraph CreateGraph(AnalysisMetadata metadata)
	{
		var graph = new HandlersGraph();
		var analyzers = metadata.Handlers.Select
		(
			handlerMetadata => new HandlerAnalyzer(handlerMetadata)
		);

		foreach (var handlerAnalyzer in analyzers)
		{
			graph.RenderHandler(handlerAnalyzer);
		}

		return graph;
	}
}