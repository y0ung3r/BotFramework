using System.Collections.Generic;

namespace BotFramework.Previewer.HandlersMetadata;

/// <summary>
/// Представляет метаданные для анализа
/// </summary>
public sealed class AnalysisMetadata
{
	/// <summary>
	/// Метаданные анализируемых обработчиков
	/// </summary>
	public IReadOnlyCollection<HandlerMetadata> Handlers { get; set; }
}