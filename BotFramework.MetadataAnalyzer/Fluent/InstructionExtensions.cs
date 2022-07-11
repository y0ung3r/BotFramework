using Mono.Cecil;
using Mono.Cecil.Cil;

namespace BotFramework.MetadataAnalyzer.Fluent;

/// <summary>
/// Методы-расширения для <see cref="Instruction"/>
/// </summary>
internal static class InstructionExtensions
{
	/// <summary>
	/// Применяет к списку IL инструкций фильтрацию по виртуальным вызовам, имеющие указанное имя
	/// </summary>
	/// <param name="instructions">IL инструкции</param>
	/// <param name="name">Имя операнда</param>
	/// <typeparam name="TOperand">Тип операнда</typeparam>
	/// <returns></returns>
	public static IEnumerable<TOperand> WithVirtualCalls<TOperand>(this IEnumerable<Instruction> instructions, string name)
		where TOperand : MethodSpecification
	{
		return instructions.Where
		(
			instruction => instruction.OpCode == OpCodes.Callvirt
		)
		.Select(instruction => instruction.Operand)
		.OfType<TOperand>()
		.Where(operand => operand.Name == name);
	}
}