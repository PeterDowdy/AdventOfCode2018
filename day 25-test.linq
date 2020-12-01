<Query Kind="Program" />

void Main()
{
	//2
	var case1 = @"0,0,0,0
 3,0,0,0
 0,3,0,0
 0,0,3,0
 0,0,0,3
 0,0,0,6
 9,0,0,0
12,0,0,0";
	//4
	var case2 = @"-1,2,2,0
0,0,2,-2
0,0,0,-2
-1,2,0,0
-2,-2,-2,2
3,0,2,-1
-1,3,2,2
-1,0,-1,0
0,2,1,-2
3,0,0,0";
//3
	var case3 = @"1,-1,0,1
2,0,-1,0
3,2,-1,0
0,0,3,1
0,0,-1,-1
2,3,-2,0
-2,2,0,0
2,-2,0,-1
1,-1,0,-1
3,2,0,2";
	//8
	var case4 = @"1,-1,-1,-2
-2,-2,0,1
0,2,1,3
-2,3,-2,1
0,2,3,-2
-1,-1,1,-2
0,-2,-1,0
-2,2,3,-1
1,2,2,0
-1,-2,0,-2";
	var points = case3.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Split(',').Select(int.Parse).ToArray()).ToList();

	var constellations = new List<List<int[]>>();
	constellations.Add(new List<int[]> { points.First() });
	points = points.Skip(1).ToList();
	while (points.Any())
	{
		if (constellations.Count(c => c.Any(d => ManhattanDistance(d, points.First()) <= 3)) > 1) {
			var original = constellations.First(c => c.Any(d => ManhattanDistance(d, points.First()) <= 3));
			var toRemove = new List<List<int[]>>();
			foreach (var constellation in constellations.Where(c => c != original && c.Any(d => ManhattanDistance(d, points.First()) <= 3)))
			{
				original.AddRange(constellation);
				toRemove.Add(constellation);
			}
			foreach (var x in toRemove) constellations.Remove(x);
			original.Add(points.First());
		}
		else if (constellations.Count(c => c.Any(d => ManhattanDistance(d, points.First()) <= 3)) == 1)
		{
			constellations.First(c => c.Any(d => ManhattanDistance(d, points.First()) <= 3)).Add(points.First());
		}
		else
		{
			constellations.Add(new List<int[]> { points.First() });
		}
		points = points.Skip(1).ToList();
	}
	constellations.Count().Dump();
}

private int ManhattanDistance(int[] a, int[] b) {
	var distance = 0;
	for (var i = 0; i < a.Length; i++) {
		distance += Math.Abs(a[i]-b[i]);
	}
	return distance;
}

// Define other methods and classes here
