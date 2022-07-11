namespace BotFramework.MetadataAnalyzer.Exceptions;

/// <summary>
/// Представляет исключение, которое вызывается, если не удалось загрузить сборку
/// </summary>
public class HandlerAssemblyNotLoadedException : ArgumentException
{
	/// <summary>
	/// Инициализирует <see cref="HandlerAssemblyNotLoadedException"/>
	/// </summary>
	public HandlerAssemblyNotLoadedException()
		: base("Не удалось загрузить сборку")
	{ }
}