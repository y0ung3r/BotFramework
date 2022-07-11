using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BotFramework.Previewer.HandlersMetadata.Extensions;

/// <summary>
/// Методы расширения для <see cref="IEnumerable{T}"/>
/// </summary>
public static class EnumerableExtensions
{
	/// <summary>
	/// Получает метаданные для анализа, используя указанные типы обработчиков
	/// </summary>
	/// <param name="types">Типы обработчиков</param>
	public static AnalysisMetadata ToAnalysisMetadata(this IEnumerable<Type> types)
	{
		var handlersMetadata = types.Select
		(
			type => new HandlerMetadata(type)
		)
		.ToList();

		return new AnalysisMetadata
		{
			Handlers = new ReadOnlyCollection<HandlerMetadata>(handlersMetadata)
		};
	}
}