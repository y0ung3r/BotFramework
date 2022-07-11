using BotFramework.Context.Interfaces;
using BotFramework.Handlers.Interfaces;
using BotFramework.MetadataAnalyzer.Exceptions;
using BotFramework.MetadataAnalyzer.Fluent;
using BotFramework.MetadataAnalyzer.Interfaces;
using BotFramework.Previewer.HandlersMetadata;
using Mono.Cecil;

namespace BotFramework.MetadataAnalyzer;

/// <summary>
/// Представляет реализацию для <see cref="IHandlerAnalyzer"/>
/// </summary>
public class HandlerAnalyzer : IHandlerAnalyzer
{
	/// <summary>
	/// Наименование метода по которому необходимо определить тип обновления
	/// </summary>
	private const string HandleMethod = nameof(IUpdateHandler<object, object>.HandleAsync);
	
	/// <summary>
	/// Наименование метода вызова планировщика
	/// </summary>
	private const string SchedulerMethod = nameof(IBotContext<object>.WaitNextUpdateAsync);
	
	/// <summary>
	/// Наименование метода, который определяет условия запуска обработчика
	/// </summary>
	private const string PrerequisiteMethod = nameof(IWithAsyncPrerequisite<object>.CanHandleAsync);
	
	/// <summary>
	/// Десериализованный .dll сборки
	/// </summary>
	private readonly ModuleDefinition _handlerModule;

	/// <inheritdoc />
	public HandlerMetadata Metadata { get; }

	/// <inheritdoc />
	public string GetUpdateName()
	{
		var method = HandlerType.GetMethod(HandleMethod);
		var parameter = method.Parameters.First();
		
		return parameter.ParameterType.Name;
	}

	/// <inheritdoc />
	public bool IsPrerequisiteDefined()
	{
		return HandlerType.GetMethod(PrerequisiteMethod) != null;
	}

	/// <inheritdoc />
	public Queue<SchedulerCallingDefinition> GetSchedulerCalls()
	{
		var callingDefinitions = new List<SchedulerCallingDefinition>();
		
		var instructions = HandlerType.GetAllInstructions();
		var schedulerCalls = instructions.WithVirtualCalls<GenericInstanceMethod>(SchedulerMethod);

		foreach (var schedulerCall in schedulerCalls)
		{
			var genericArgument = schedulerCall.GenericArguments.First();
			var updateName = genericArgument.Name;
			
			callingDefinitions.Add
			(
				new SchedulerCallingDefinition(updateName)
			);
		}

		return new Queue<SchedulerCallingDefinition>(callingDefinitions);
	}

	/// <summary>
	/// Тип обработчика
	/// </summary>
	private TypeDefinition HandlerType => _handlerModule.Types.GetType(Metadata.HandlerNamespace);

	/// <summary>
	/// Инициализирует <see cref="HandlerAnalyzer"/>
	/// </summary>
	/// <param name="metadata">Метаданные для анализа обработчика</param>
	/// <exception cref="HandlerAssemblyNotLoadedException">Сборка обработчика не была загружена</exception>
	public HandlerAnalyzer(HandlerMetadata metadata)
	{
		ArgumentNullException.ThrowIfNull(metadata);
		
		Metadata = metadata;
		
		try
		{
			var handlerAssembly = AssemblyDefinition.ReadAssembly(Metadata.ModulePath);
			_handlerModule = handlerAssembly.MainModule;
		}

		catch (ArgumentException)
		{
			throw new HandlerAssemblyNotLoadedException();
		}
	}
}