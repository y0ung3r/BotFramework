namespace BotFramework.Previewer.Interfaces;

/// <summary>
/// Представляет запуск BotFramework Previewer
/// </summary>
public interface IPreviewerRunner
{
	/// <summary>
	/// Сериализированные метаданные
	/// </summary>
	string MetadataSchema { get; }
	
	/// <summary>
	/// Запускает BotFramework Previewer
	/// </summary>
	public void Run();
}