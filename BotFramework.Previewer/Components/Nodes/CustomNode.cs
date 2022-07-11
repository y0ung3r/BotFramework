namespace BotFramework.Previewer.Components.Nodes;

public abstract class CustomNode
{
	public string Name { get; }
	
	protected CustomNode()
	{ }
	
	public CustomNode(string name)
	{
		Name = name;
	}
}