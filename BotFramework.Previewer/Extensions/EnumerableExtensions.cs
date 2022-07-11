namespace BotFramework.Previewer.Extensions;

public static class EnumerableExtensions
{
	public static void ForEach<TSource>(this IEnumerable<TSource> enumerable, Action<TSource, TSource> action)
		where TSource : class
	{
		ArgumentNullException.ThrowIfNull(enumerable);
		ArgumentNullException.ThrowIfNull(action);
		
		var elementsCount = enumerable.Count();

		for (var index = 0; index < elementsCount; index++)
		{
			var left = enumerable.ElementAt(index);
			var right = enumerable.ElementAtOrDefault(index + 1);
			action(left, right);
		}
	}
}