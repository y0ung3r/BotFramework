using System;

namespace BotFramework.Previewer.HandlersMetadata;

/// <summary>
/// Представляет метаданные обработчика для анализа
/// </summary>
public sealed class HandlerMetadata
{
	/// <summary>
	/// Путь к .dll, в котором содержится обработчик
	/// </summary>
	public string ModulePath { get; set; }

	/// <summary>
	/// Наименование типа обработчика, включая сборку
	/// </summary>
	public string HandlerAssembly { get; set; }
	
	/// <summary>
	/// Короткое наименование типа обработчика
	/// </summary>
	public string HandlerName { get; set; }
	
	/// <summary>
	/// Наименование типа обработчика, включая Namespace
	/// </summary>
	public string HandlerNamespace { get; set; }
	
	/// <summary>
	/// Инициализирует <see cref="HandlerMetadata"/>.
	/// Используется сериализатором
	/// </summary>
	public HandlerMetadata() 
	{ }

	/// <summary>
	/// Инициализирует <see cref="HandlerMetadata"/>
	/// </summary>
	/// <param name="handlerType">Тип обработчика</param>
	public HandlerMetadata(Type handlerType)
	{
		ArgumentNullException.ThrowIfNull(handlerType);
		
		ModulePath = handlerType.Module.FullyQualifiedName;
		HandlerAssembly = handlerType.AssemblyQualifiedName;
		HandlerName = handlerType.Name;
		HandlerNamespace = handlerType.FullName;
	}
}