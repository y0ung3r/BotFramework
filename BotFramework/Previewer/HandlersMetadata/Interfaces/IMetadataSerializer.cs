namespace BotFramework.Previewer.HandlersMetadata.Interfaces;

/// <summary>
/// Определяет сериализатор для <see cref="AnalysisMetadata"/>
/// </summary>
public interface IMetadataSerializer
{
	/// <summary>
	/// Возвращает сериализованные метаданные для анализа
	/// </summary>
	/// <param name="metadata">Метаданные для анализа</param>
	public string Serialize(AnalysisMetadata metadata);

	/// <summary>
	/// Возвращает десериализованные метаданные для анализа
	/// </summary>
	/// <param name="schema">Схема метаданных для анализа</param>
	public AnalysisMetadata Deserialize(string schema);
}