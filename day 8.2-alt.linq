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
	HeaderNode finalTree = BuildTree(tokens);
	EatTree(finalTree).Dump();
	//Should be 32850
}
public HeaderNode BuildTree(Queue<int> nodes)
{
	var thisNode = new HeaderNode
	{
		ChildCount = nodes.Dequeue(),
		MetadataCount = nodes.Dequeue(),
		Children = new List<UserQuery.HeaderNode>(),
		Metadata = new List<int>()
	};
	for (var i = 0; i < thisNode.ChildCount; i++)
	{
		thisNode.Children.Add(BuildTree(nodes));
	}
	for (var i = 0; i < thisNode.MetadataCount; i++)
	{
		thisNode.Metadata.Add(nodes.Dequeue());
	}
	return thisNode;
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