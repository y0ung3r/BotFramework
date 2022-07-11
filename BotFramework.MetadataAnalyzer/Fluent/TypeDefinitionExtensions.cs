using Mono.Cecil;
using Mono.Cecil.Cil;

namespace BotFramework.MetadataAnalyzer.Fluent;

/// <summary>
/// Представляет методы-расширения для <see cref="TypeDefinition"/>
/// </summary>
internal static class TypeDefinitionExtensions
{
	/// <summary>
	/// Возвращает определение типа, используя указанное полное имя (включая Namespace)
	/// </summary>
	/// <param name="typeDefinitions">Определения типов</param>
	/// <param name="name">Полное имя (включая Namespace)</param>
	public static TypeDefinition GetType(this IEnumerable<TypeDefinition> typeDefinitions, string name)
	{
		return typeDefinitions.FirstOrDefault
		(
			moduleType => moduleType.FullName == name
		);
	}

	/// <summary>
	/// Возвращает метод, определенный в указанном типе, используя заданное имя
	/// </summary>
	/// <param name="typeDefinition">Определения типов</param>
	/// <param name="name">Полное имя метода</param>
	public static MethodDefinition GetMethod(this TypeDefinition typeDefinition, string name)
	{
		return typeDefinition.Methods.FirstOrDefault
		(
			method => method.Name == name
		);
	}

	/// <summary>
	/// Возвращает основные IL инструкции
	/// </summary>
	/// <param name="typeDefinition">Определение типа</param>
	public static IEnumerable<Instruction> GetMainInstructions(this TypeDefinition typeDefinition)
	{
		return typeDefinition.Methods.SelectMany
		(
			method => method.Body.Instructions
		);
	}

	/// <summary>
	/// Возвращает IL инструкции вложенных типов
	/// </summary>
	/// <param name="typeDefinition">Определение типа</param>
	public static IEnumerable<Instruction> GetNestedInstructions(this TypeDefinition typeDefinition)
	{
		var methods = typeDefinition.NestedTypes.SelectMany(nestedType => nestedType.Methods);
		return methods.SelectMany(method => method.Body.Instructions);
	}

	/// <summary>
	/// Возвращает все IL инструкции (основные и вложенные)
	/// </summary>
	/// <param name="typeDefinition">Определение типа</param>
	public static IEnumerable<Instruction> GetAllInstructions(this TypeDefinition typeDefinition)
	{
		var mainInstructions = typeDefinition.GetMainInstructions();
		var nestedInstructions = typeDefinition.GetNestedInstructions();
		return mainInstructions.Concat(nestedInstructions).ToList();
	}
}