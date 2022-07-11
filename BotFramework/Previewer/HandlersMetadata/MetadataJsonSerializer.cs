using System.Text.Json;
using BotFramework.Previewer.HandlersMetadata.Interfaces;

namespace BotFramework.Previewer.HandlersMetadata;

/// <summary>
/// Реализация Json-сериализатора для <see cref="IMetadataSerializer"/>
/// </summary>
internal class MetadataJsonSerializer : IMetadataSerializer
{
	/// <summary>
	/// Возвращает сериализованные метаданные для анализа
	/// </summary>
	/// <param name="metadata">Метаданные для анализа</param>
	public string Serialize(AnalysisMetadata metadata) => JsonSerializer.Serialize(metadata);

	/// <summary>
	/// Возвращает десериализованные метаданные для анализа
	/// </summary>
	/// <param name="schema">Схема метаданных для анализа</param>
	public AnalysisMetadata Deserialize(string schema) => JsonSerializer.Deserialize<AnalysisMetadata>(schema);
}