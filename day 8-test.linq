<Query Kind="Program" />

public class HeaderNode
{
	public int ChildCount {get; set;}
	public int MetadataCount {get; set;}
	public List<HeaderNode> Children { get; set; }
	public List<int> Metadata {get; set;}
}
void Main()
{	
	var testInput = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2";
	var testTokens = new Queue<int>(testInput.Split(' ').Select(int.Parse).ToList());
	var treeStack = new Stack<HeaderNode>();
	HeaderNode finalTree = null;
	var metadataCount = 0;
	while (testTokens.Any())
	{
		if (treeStack.Any())
		{
			var top = treeStack.Pop();
			if (top.ChildCount == top.Children.Count)
			{
				for (var i = 0; i < top.MetadataCount; i++)
				{
					top.Metadata.Add(testTokens.Dequeue());
				}
				metadataCount += top.Metadata.Sum();
				if (treeStack.Any())
				{
					var parent = treeStack.Pop();
					parent.Children.Add(top);
					treeStack.Push(parent);
				}
				else
				{
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
			ChildCount = testTokens.Dequeue(),
			MetadataCount = testTokens.Dequeue(),
			Children = new List<UserQuery.HeaderNode>(),
			Metadata = new List<int>()
		};
		if (nextNode.ChildCount == 0)
		{
			var top = treeStack.Pop();
			for (var i = 0; i < nextNode.MetadataCount; i++)
			{
				nextNode.Metadata.Add(testTokens.Dequeue());
			}
			metadataCount += nextNode.Metadata.Sum();
			top.Children.Add(nextNode);
			treeStack.Push(top);
		} else {
			treeStack.Push(nextNode);
		}
	}
	metadataCount.Dump();
}