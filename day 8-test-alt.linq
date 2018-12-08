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
	HeaderNode finalTree = BuildTree(testTokens);	
	EatTree(finalTree).Dump();
	//Should be 138
}
public HeaderNode BuildTree(Queue<int> nodes) {
	var thisNode = new HeaderNode
	{
		ChildCount = nodes.Dequeue(),
		MetadataCount = nodes.Dequeue(),
		Children = new List<UserQuery.HeaderNode>(),
		Metadata = new List<int>()
	};
	for (var i =0; i < thisNode.ChildCount; i++) {
		thisNode.Children.Add(BuildTree(nodes));
	}
	for (var i = 0; i < thisNode.MetadataCount; i++)
	{
		thisNode.Metadata.Add(nodes.Dequeue());
	}
	return thisNode;
}
public int EatTree(HeaderNode node)
{
	if (node.ChildCount == 0)
	{
		return node.Metadata.Sum();
	}
	return node.Metadata.Sum() + node.Children.Select(EatTree).Sum();	
}