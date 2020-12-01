<Query Kind="Program" />

const int depth = 510;
public enum Terrain
{
	Rocky,
	Wet,
	Narrow
}
public enum Tool
{
	Torch,
	ClimbingGear,
	Neither
}
public int[,] Erosions = new int[10 * 10, 10 * 10];
void Main()
{
	var target = (X: 10, Y: 10);
	Erosions[0, 0] = (0 + depth) % 20183;
	Erosions[target.Y, target.X] = (0 + depth) % 20183;
	var path = BuildPath((0, 0, Tool.Torch), (target.X, target.Y, Tool.Torch));
	var timeSpent = 0;
	var prev = path.First();
	foreach (var node in path.Skip(1))
	{
		Console.WriteLine(node.tool == prev.tool ? $"Moving to {node.X},{node.Y}" : $"Switching tool to {node.tool}");
		timeSpent += node.tool == prev.tool ? 1 : 7;
		prev = node;
	}
	Console.WriteLine($"This took {timeSpent} minutes.");
}
//Coped from The Internet
private class PathNode
{
	public int X;
	public int Y;
	public Tool Tool;
	public double F = Double.PositiveInfinity;
	public double G = Double.PositiveInfinity;
	public PathNode Parent;
}
private List<(int X, int Y, Tool tool)> BuildPath((int X, int Y, Tool Tool) start, (int X, int Y, Tool Tool) target)
{
	PathNode current = null;
	var openList = new List<PathNode>();
	var closedList = new List<PathNode>();

	// start by adding the original position to the open list
	openList.Add(new PathNode { X = start.X, Y = start.Y, Tool = start.Tool, G = 0, F = ComputeHScore(start.X, start.Y, target.X, target.Y) });

	while (openList.Any())
	{
		// get the square with the lowest F score
		current = openList.OrderBy(l => l.F).First();
		if (current.X == target.X && current.Y == target.Y && current.Tool == target.Tool) break;

		// remove it from the open list
		openList.Remove(current);
		// add the current square to the closed list
		closedList.Add(current);

		var adjacentSquares = GetWalkableAdjacentSquares((current.X, current.Y, current.Tool));
		//var thisRoom = rooms[(current.X, current.Y, current.Tool)];
		foreach (var adjacentSquare in adjacentSquares)
		{
			// if this adjacent square is already in the closed list, ignore it
			if (closedList.Any(l => l.X == adjacentSquare.X && l.Y == adjacentSquare.Y && l.Tool == adjacentSquare.Tool)) continue;
			// It costs 7 minutes to move from tool to tool and 1 to move from square to square
			var tentantiveGScore = current.G + (current.Tool != adjacentSquare.Tool ? 7 : 1);
			// if it's not in the open list...
			if (!openList.Any(l => l.X == adjacentSquare.X && l.Y == adjacentSquare.Y && l.Tool == adjacentSquare.Tool))
			{
				// add it to the open list
				openList.Add(adjacentSquare);
			}
			if (tentantiveGScore < adjacentSquare.G)
			{
				// this is better than the previous route; take it.
				adjacentSquare.G = tentantiveGScore;
				adjacentSquare.F = tentantiveGScore + ComputeHScore(adjacentSquare.X, adjacentSquare.Y, target.X, target.Y);
				adjacentSquare.Parent = current;
			}
		}
	}
	// assume path was found; let's show it
	var bestpath = new List<(int X, int Y, Tool tool)>();
	var bestNodePath = new List<PathNode>();
	if (current.X != target.X || current.Y != target.Y || current.Tool != target.Tool) return new List<(int X, int Y, Tool tool)>();
	while (current != null)
	{
		bestpath.Add((current.X, current.Y, current.Tool));
		bestNodePath.Add(current);
		current = current.Parent;
	}
	bestpath.Reverse();
	bestNodePath.Reverse();
	return bestpath;
}
List<PathNode> GetWalkableAdjacentSquares((int X, int Y, Tool Tool) room)
{
	var proposedLocations = new List<PathNode>()
			{
				new PathNode { X = room.X-1, Y = room.Y, Tool = room.Tool},
				new PathNode { X = room.X, Y = room.Y+1, Tool = room.Tool},
				new PathNode { X = room.X+1, Y = room.Y, Tool = room.Tool},
				new PathNode { X = room.X, Y = room.Y-1, Tool = room.Tool},
				new PathNode { X = room.X, Y = room.Y, Tool = Tool.Neither},
				new PathNode { X = room.X, Y = room.Y, Tool = Tool.Torch},
				new PathNode { X = room.X, Y = room.Y, Tool = Tool.ClimbingGear},
			};
	var filteredLocations = proposedLocations.Where(l => l.X >= 0 && l.Y >= 0).Where(r =>
	{
		if (r.Tool == room.Tool && r.X == room.X && r.Y == room.Y) return false;
		var adjacentErosionLevel = GetErosionLevel((r.X, r.Y));
		switch ((Terrain)(adjacentErosionLevel % 3))
		{
			case Terrain.Narrow:
				if (r.Tool == Tool.ClimbingGear) return false;
				break;
			case Terrain.Rocky:
				if (r.Tool == Tool.Neither) return false;
				break;
			case Terrain.Wet:
				if (r.Tool == Tool.Torch) return false;
				break;
		}
		return true;
	}).ToList();
	return filteredLocations;
}
static int ComputeHScore(int x, int y, int targetX, int targetY)
{
	return Math.Abs(targetX - x) + Math.Abs(targetY - y);
}
public int GetErosionLevel((int X, int Y) index)
{
	if (index.X == 0)
	{
		return (48271 * index.Y + depth) % 20183;
	}
	else if (index.Y == 0)
	{
		return (16807 * index.X + depth) % 20183;
	}
	else
	{
		if (Erosions[index.Y,index.X-1] == 0)
		{
			Erosions[index.Y,0] = (48271 * index.Y + depth) % 20183;
			for (var x = 1; x < index.X; x++)
			{
				if (Erosions[index.Y, x] == 0) Erosions[index.Y,x] = (Erosions[index.Y,x-1]*Erosions[index.Y-1,x]+depth)%20183;
			}
		}
		if (Erosions[index.Y - 1, index.X] == 0)
		{
			Erosions[0, index.X] = (16807 * index.X + depth) % 20183;
			for (var y = 1; y < index.Y; y++)
			{
				if (Erosions[y, index.X] == 0) Erosions[y, index.X] = (Erosions[y, index.X - 1] * Erosions[y- 1, index.X] + depth) % 20183;
			}
		}
		if (Erosions[index.Y,index.X] == 0) {
			Erosions[index.Y, index.X] = (Erosions[index.Y, index.X - 1] * Erosions[index.Y - 1, index.X] + depth) % 20183;
		}
		return Erosions[index.Y,index.X];
	}
}
