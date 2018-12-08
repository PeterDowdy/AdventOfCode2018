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
	var metadataCount = 0;
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
				metadataCount += top.Metadata.Sum();
				if (treeStack.Any())
				{
					var parent = treeStack.Pop();
					parent.Children.Add(top);
					treeStack.Push(parent);
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
			metadataCount += nextNode.Metadata.Sum();
			top.Children.Add(nextNode);
			treeStack.Push(top);
		}
		else
		{
			treeStack.Push(nextNode);
		}
	}
	metadataCount.Dump();
	//Should be 48496
}