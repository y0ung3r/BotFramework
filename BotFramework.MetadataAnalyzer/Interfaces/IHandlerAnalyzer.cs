using BotFramework.Previewer.HandlersMetadata;

namespace BotFramework.MetadataAnalyzer.Interfaces;

/// <summary>
/// Определяет анализатор IL-кода обработчика
/// </summary>
public interface IHandlerAnalyzer
{
	/// <summary>
	/// Метаданные обработчика для анализа
	/// </summary>
	HandlerMetadata Metadata { get; }

	/// <summary>
	/// Возвращает наименование типа обновления, который указан в обработчике
	/// </summary>
	string GetUpdateName();
	
	/// <summary>
	/// Проверяет существование метода, который определяет условия запуска обработчика
	/// </summary>
	bool IsPrerequisiteDefined();

	/// <summary>
	/// Возвращает список асинхронных вызовов планировщика 
	/// </summary>
	public Queue<SchedulerCallingDefinition> GetSchedulerCalls();
}