<Query Kind="Program" />

public class HeaderNode
{
	public int ChildCount { get; set; }
	public int MetadataCount { get; set; }
	public List<HeaderNode> Children { get; set; }
	public List<int> Metadata { get; set; }
}
void Main()
{
	var input = File.ReadAllText($"{Path.GetDirectoryName(Util.CurrentQueryPath)}/8.txt");
	var tokens = new Queue<int>(input.Split(' ').Select(int.Parse).ToList());
	var treeStack = new Stack<HeaderNode>();
	HeaderNode finalTree = null;
	while (tokens.Any())
	{
		if (treeStack.Any())
		{
			var top = treeStack.Pop();
			if (top.ChildCount == top.Children.Count)
			{
				for (var i = 0; i < top.MetadataCount; i++)
				{
					top.Metadata.Add(tokens.Dequeue());
				}
				if (treeStack.Any())
				{
					var parent = treeStack.Pop();
					parent.Children.Add(top);
					treeStack.Push(parent);
				}
				else {
					finalTree = top;
				}
				continue;
			}
			else
			{
				treeStack.Push(top);
			}
		}
		var nextNode = new HeaderNode
		{
			ChildCount = tokens.Dequeue(),
			MetadataCount = tokens.Dequeue(),
			Children = new List<UserQuery.HeaderNode>(),
			Metadata = new List<int>()
		};
		if (nextNode.ChildCount == 0)
		{
			var top = treeStack.Pop();
			for (var i = 0; i < nextNode.MetadataCount; i++)
			{
				nextNode.Metadata.Add(tokens.Dequeue());
			}
			top.Children.Add(nextNode);
			treeStack.Push(top);
		}
		else
		{
			treeStack.Push(nextNode);
		}
	}
	EatTree(finalTree).Dump();
}
public int EatTree(HeaderNode node) {
	if (node.ChildCount == 0) {
		return node.Metadata.Sum();
	}
	var edibleChildren = new List<HeaderNode>();
	for (var i = 0; i < node.MetadataCount; i++) {
		if (node.Metadata[i]-1 < node.ChildCount) edibleChildren.Add(node.Children[node.Metadata[i]-1]);
	}
	return edibleChildren.Select(EatTree).Sum();
}