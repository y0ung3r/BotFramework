using BotFramework.MetadataAnalyzer;
using BotFramework.Previewer.Components.Nodes;

namespace BotFramework.Previewer.Extensions;

public static class HandlerAnalyzerExtensions
{
	public static HandlerNode ToHandlerNode(this HandlerAnalyzer analyzer)
	{
		var nestedNodes = new Queue<OperationNode>();
		var schedulerCalls = analyzer.GetSchedulerCalls();
		
		foreach (var operation in schedulerCalls)
		{
			var operationNode = new OperationNode(operation.UpdateName);
			nestedNodes.Enqueue(operationNode);
		}

		return new HandlerNode(analyzer.Metadata.HandlerName)
		{
			UpdateName = analyzer.GetUpdateName(),
			IsPrerequisiteDefined = analyzer.IsPrerequisiteDefined(),
			NestedNodes = nestedNodes
		};
	}
}