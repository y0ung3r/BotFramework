namespace BotFramework.MetadataAnalyzer;

/// <summary>
/// Представляет вызов планировщика
/// </summary>
public class SchedulerCallingDefinition
{
	/// <summary>
	/// Наименование типа обновления
	/// </summary>
	public string UpdateName { get; }

	/// <summary>
	/// Инициализирует <see cref="SchedulerCallingDefinition"/>
	/// </summary>
	/// <param name="updateName">Наименование типа обновления</param>
	public SchedulerCallingDefinition(string updateName)
	{
		ArgumentNullException.ThrowIfNull(updateName);
		
		UpdateName = updateName;
	}
}