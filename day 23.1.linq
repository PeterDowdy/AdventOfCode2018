<Query Kind="Program" />

void Main()
{
	var input = File.ReadAllLines($"{Path.GetDirectoryName(Util.CurrentQueryPath)}/23.txt").Select(x =>
	{
		var tokens = x.Replace("pos=<","").Replace(">","").Replace(" r=", "").Split(',');
		return (X: int.Parse(tokens[0]), Y: int.Parse(tokens[1]), Z: int.Parse(tokens[2]), R: int.Parse(tokens[3]));
	});
	var strongest = input.OrderByDescending(i => i.R).First();
	var nanobotsInRange = input.Where(i => {
		return (Math.Abs(i.X-strongest.X) + Math.Abs(i.Y-strongest.Y) + Math.Abs(i.Z-strongest.Z)) <= strongest.R;
	}).ToList();
	nanobotsInRange.Count.Dump();
}