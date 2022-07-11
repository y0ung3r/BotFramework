namespace BotFramework.Previewer.Components.Nodes;

public class HandlerNode : CustomNode
{
	public string Header
	{
		get
		{
			var header = Name;

			if (!string.IsNullOrWhiteSpace(UpdateName))
			{
				header += $" ({UpdateName})";
			}
			
			return header;
		}
	}

	public string UpdateName { get; init; }
	
	public bool IsPrerequisiteDefined { get; init; }
	
	public Queue<OperationNode> NestedNodes { get; init; }

	public HandlersGraph NestedGraph { get; }

	public HandlerNode(string name)
		: base(name)
	{
		NestedGraph = new HandlersGraph();
		NestedNodes = new Queue<OperationNode>();
	}
}