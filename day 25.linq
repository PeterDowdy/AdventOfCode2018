<Query Kind="Program" />

void Main()
{
	var points = File.ReadAllLines($"{Path.GetDirectoryName(Util.CurrentQueryPath)}/25.txt").Select(x => x.Split(',').Select(int.Parse).ToArray()).ToList();

	var constellations = new List<List<int[]>>();
	constellations.Add(new List<int[]> { points.First() });
	points = points.Skip(1).ToList();
	while (points.Any())
	{
		if (constellations.Any(c => c.Any(d => ManhattanDistance(d, points.First()) <= 3)))
		{
			if (constellations.Count(c => c.Any(d => ManhattanDistance(d, points.First()) <= 3)) > 1) {
				var original = constellations.First(c => c.Any(d => ManhattanDistance(d, points.First()) <= 3));
				var toRemove = new List<List<int[]>>();
				foreach (var constellation in constellations.Where(c => c != original && c.Any(d => ManhattanDistance(d, points.First()) <= 3))) {
					original.AddRange(constellation);
					toRemove.Add(original);
				}
				foreach (var x in toRemove) constellations.Remove(x);
				original.Add(points.First());
			}
			else constellations.First(c => c.Any(d => ManhattanDistance(d, points.First()) <= 3)).Add(points.First());
		}
		else
		{
			constellations.Add(new List<int[]> { points.First() });
		}
		points = points.Skip(1).ToList();
	}
	var merged = false;
	do
	{
		merged = false;
		List<int[]> toDelete = null;
		foreach (var constellation in constellations)
		{
			foreach (var comparisonConstellation in constellations) {
				if (constellation == comparisonConstellation) continue;
				if (constellation.Any(c => comparisonConstellation.Any(d => ManhattanDistance(c, d) <= 3)))
				{
					comparisonConstellation.AddRange(constellation);
					toDelete = constellation;
					merged = true;
					break;
				}
				if (merged) break;
			}
		}
		if (toDelete != null) constellations.Remove(toDelete);
	} while (merged);
	constellations.Count().Dump();
}

private int ManhattanDistance(int[] a, int[] b)
{
	var distance = 0;
	for (var i = 0; i < a.Length; i++)
	{
		distance += Math.Abs(a[i] - b[i]);
	}
	return distance;
}

// Define other methods and classes here
