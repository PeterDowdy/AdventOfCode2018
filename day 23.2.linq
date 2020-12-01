<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.ComponentModel.DataAnnotations.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>System.ComponentModel.DataAnnotations.Schema</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
</Query>

public class OctNode {
	public readonly (long X, long Y, long Z) Node; //smallest corner
	public readonly long Size; //in each dimension
	public readonly long Overlaps;
	public OctNode ((long X, long Y, long Z) node, long size, long overlaps) {
		Node = node;
		Size = size;
		Overlaps = overlaps;
	}
}

void Main()
{
	var input = File.ReadAllLines(@"E:\code\AdventOfCode2018\23.txt")
	//var input = new[] {"pos=<10,12,12>, r=2","pos=<12,14,12>, r=2","pos=<16,12,12>, r=4","pos=<14,14,14>, r=6","pos=<50,50,50>, r=200","pos=<10,10,10>, r=5"}
	.Select(x =>
		{
			var tokens = x.Replace("pos=<", "").Replace(">", "").Replace(" r=", "").Split(',');
			return (X: long.Parse(tokens[0]), Y: long.Parse(tokens[1]), Z: long.Parse(tokens[2]), R: long.Parse(tokens[3]));
		}).ToList();
		var max = new[] { input.Max(i => i.X) - input.Min(i => i.X), input.Max(i => i.Y) - input.Min(i => i.Y), input.Max(i => i.Z) - input.Min(i => i.Z)}.Max();
		var size = 1;
		while (size < max) size *=2;
	var q = new List<OctNode> { new OctNode((input.Min(i => i.X), input.Min(i => i.Y), input.Min(i => i.Z)), size, input.Count)};
	while (q.Any())
	{
		var top = q.First();
		q = q.Skip(1).ToList();
		if (top.Size == 1)
		{
			Console.WriteLine($"Answer found: ({top.Node.X},{top.Node.Y},{top.Node.Z}), overlaps: {top.Overlaps}, distance: {ManhattanDistance(top.Node, (0, 0, 0))}");
			break;
		}
		q.AddRange(new[] {
			new OctNode((top.Node.X, top.Node.Y, top.Node.Z), top.Size/2, input.Count(i => NodeInSearchSpace((top.Node.X, top.Node.Y, top.Node.Z), top.Size/2, i))),
			new OctNode((top.Node.X+(top.Size/2), top.Node.Y, top.Node.Z), top.Size/2,input.Count(i => NodeInSearchSpace((top.Node.X+(top.Size/2), top.Node.Y, top.Node.Z), top.Size/2, i))),
			new OctNode((top.Node.X, top.Node.Y+(top.Size/2), top.Node.Z), top.Size/2,input.Count(i => NodeInSearchSpace((top.Node.X, top.Node.Y+(top.Size/2), top.Node.Z), top.Size/2, i))),
			new OctNode((top.Node.X+(top.Size/2), top.Node.Y+(top.Size/2), top.Node.Z), top.Size/2,input.Count(i => NodeInSearchSpace((top.Node.X+(top.Size/2), top.Node.Y+(top.Size/2), top.Node.Z), top.Size/2, i))),
			new OctNode((top.Node.X, top.Node.Y, top.Node.Z+(top.Size/2)), top.Size/2,input.Count(i => NodeInSearchSpace((top.Node.X, top.Node.Y, top.Node.Z+(top.Size/2)), top.Size/2, i))),
			new OctNode((top.Node.X+(top.Size/2), top.Node.Y, top.Node.Z+(top.Size/2)), top.Size/2,input.Count(i => NodeInSearchSpace((top.Node.X+(top.Size/2), top.Node.Y, top.Node.Z+(top.Size/2)), top.Size/2, i))),
			new OctNode((top.Node.X, top.Node.Y+(top.Size/2), top.Node.Z+(top.Size/2)), top.Size/2,input.Count(i => NodeInSearchSpace((top.Node.X, top.Node.Y+(top.Size/2), top.Node.Z+(top.Size/2)), top.Size/2, i))),
			new OctNode((top.Node.X+(top.Size/2), top.Node.Y+(top.Size/2), top.Node.Z+(top.Size/2)), top.Size/2,input.Count(i => NodeInSearchSpace((top.Node.X+(top.Size/2), top.Node.Y+(top.Size/2), top.Node.Z+(top.Size/2)), top.Size/2, i)))
		});
		q = q.Where(x => x.Size > 0 && x.Overlaps > 0).OrderByDescending(x => x.Overlaps).ThenBy(x => ManhattanDistance(x.Node, (0,0,0))).ThenBy(x => x.Size).ToList();
	}
}
public bool NodeInSearchSpace((long X, long Y, long Z) search, long size, (long X, long Y, long Z, long R) node)
{
	var d = 0L;
	if (node.X > (search.X+size-1)) d += Math.Abs(node.X - (search.X+size-1));
	if (node.X < search.X) d += Math.Abs(search.X - node.X);
	if (node.Y > (search.Y+size-1)) d += Math.Abs(node.Y - (search.Y+size-1));
	if (node.Y < search.Y) d += Math.Abs(search.Y - node.Y);
	if (node.Z > (search.Z+size-1)) d += Math.Abs(node.Z - (search.Z+size-1));
	if (node.Z < search.Z) d += Math.Abs(search.Z - node.Z);
	return d <= node.R;
}
public long ManhattanDistance((long X, long Y, long Z, long R) first, (long X, long Y, long Z, long R) second)
{
	return ManhattanDistance((first.X, first.Y, first.Z), (second.X, second.Y, second.Z));
}
public long ManhattanDistance((long X, long Y, long Z, long R) first, (long X, long Y, long Z) second)
{
	return ManhattanDistance((first.X, first.Y, first.Z), (second.X, second.Y, second.Z));
}
public long ManhattanDistance((long X, long Y, long Z) first, (long X, long Y, long Z, long R) second)
{
	return ManhattanDistance((first.X, first.Y, first.Z), (second.X, second.Y, second.Z));
}
public long ManhattanDistance((long X, long Y, long Z) first, (long X, long Y, long Z) second) {
	return Math.Abs(first.X - second.X) + Math.Abs(first.Y - second.Y) + Math.Abs(first.Z - second.Z);
}