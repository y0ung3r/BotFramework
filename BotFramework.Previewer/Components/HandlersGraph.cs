using AvaloniaGraphControl;
using BotFramework.MetadataAnalyzer;
using BotFramework.Previewer.Components.Nodes;
using BotFramework.Previewer.Extensions;

namespace BotFramework.Previewer.Components;

public class HandlersGraph : Graph
{
	private static readonly CustomNode UpdateNode = new HandlerNode("Update");
	private static readonly CustomNode ReceiverNode = new HandlerNode("Receiver");

	private readonly ICollection<CustomNode> _nodes;

	public HandlersGraph()
	{
		_nodes = new List<CustomNode>();
	}
	
	public HandlersGraph(ICollection<CustomNode> nodes)
	{
		ArgumentNullException.ThrowIfNull(nodes);
		
		_nodes = nodes;
	}

	public void RenderHandler(HandlerAnalyzer analyzer)
	{
		EnsureReceiverNodeRendered();
		
		var handlerNode = analyzer.ToHandlerNode();
		var handlerEdge = new Edge(ReceiverNode, handlerNode);
		RenderEdge(handlerEdge);
		
		handlerNode.NestedNodes.ForEach
		(
			(previousNode, node) => RenderOperation(handlerNode, previousNode, node)
		);
	}

	private void RenderEdge(Edge edge)
	{
		Edges.Add(edge);
	}
	
	private void RenderOperation(HandlerNode parent, OperationNode node, OperationNode nextNode)
	{
		var edge = default(Edge);
		
		if (nextNode is not null)
		{
			edge = new Edge(node, nextNode);
		}
		else
		{
			var endNode = new EndNode();
			edge = new Edge(node, endNode);
		}
		
		parent.NestedGraph.RenderEdge(edge);
	}

	private void EnsureReceiverNodeRendered()
	{
		var isRendered = Edges.Any
		(
			edge => edge.Head.Equals(ReceiverNode) && edge.Tail.Equals(UpdateNode)
		);
		
		if (isRendered)
		{
			return;
		}
		
		var edge = new Edge(UpdateNode, ReceiverNode);
		RenderEdge(edge);
	}
}