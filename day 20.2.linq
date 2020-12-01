<Query Kind="Program" />

void Main()
{
	var regex = File.ReadAllText($"{Path.GetDirectoryName(Util.CurrentQueryPath)}/20.txt");
	var listified = Listify(regex);
	//var permutations = Permutations(listified);
	var start = new Room
	{
		X = 0,
		Y = 0,
		Upcoming = new List<List<object>> { listified }
	};
	var rooms = new Dictionary<(int X, int Y), Room> { [(0, 0)] = start };
	var needsProcessing = rooms.Where(r => r.Value.Upcoming.Any()).ToList();
	while (needsProcessing.Any())
	{
		foreach (var room in needsProcessing)
		{
			room.Value.ProcessList(rooms);
		}
		needsProcessing = rooms.Where(r => r.Value.Upcoming.Any()).ToList();
	}
	var roomsAtLeast1000Away = new HashSet<(int X, int Y)>();
	Print(rooms);
	var total = rooms.Keys.Count();
	var soFar = 0;
	foreach (var key in rooms.Keys)
	{
		soFar++;
		Console.WriteLine($"Processing {soFar} out of {total}");
		var path = BuildPath(rooms, (0, 0), (key.X, key.Y));
		if (path.Count > 1000) {
			roomsAtLeast1000Away.Add((key.X, key.Y));
		} 
	}
	roomsAtLeast1000Away.Count.Dump();
}
public class Room
{
	public int X { get; set; }
	public int Y { get; set; }
	public Room N { get; set; }
	public Room E { get; set; }
	public Room S { get; set; }
	public Room W { get; set; }
	public List<List<object>> Upcoming { get; set; } = new List<List<object>>();
	public void ProcessList(Dictionary<(int X, int Y), Room> rooms)
	{
		if (!Upcoming.Any()) return;
		var nextUpcoming = new List<List<object>>();
		foreach (var u in Upcoming)
		{
			if (!u.Any()) continue;
			if (u.First() is char c)
			{
				switch (c)
				{
					case 'N':
						N = rooms.ContainsKey((X, Y + 1)) ? rooms[(X, Y + 1)] : null;
						if (N == null)
						{
							this.N = new Room { X = X, Y = Y + 1, S = this };
							rooms[(X, Y + 1)] = this.N;
						}
						N.S = this;
						N.Upcoming.Add(u.Skip(1).ToList());
						break;
					case 'E':
						E = rooms.ContainsKey((X + 1, Y)) ? rooms[(X + 1, Y)] : null;
						if (E == null)
						{
							this.E = new Room { X = X + 1, Y = Y, W = this };
							rooms[(X + 1, Y)] = this.E;
						}
						E.W = this;
						E.Upcoming.Add(u.Skip(1).ToList());
						break;
					case 'S':
						S = rooms.ContainsKey((X, Y - 1)) ? rooms[(X, Y - 1)] : null;
						if (S == null)
						{
							this.S = new Room { X = X, Y = Y - 1, N = this };
							rooms[(X, Y - 1)] = S;
						}
						S.N = this;
						S.Upcoming.Add(u.Skip(1).ToList());
						break;
					case 'W':
						W = rooms.ContainsKey((X - 1, Y)) ? rooms[(X - 1, Y)] : null;
						if (W == null)
						{
							this.W = new Room { X = X - 1, Y = Y, E = this };
							rooms[(X - 1, Y)] = this.W;
						}
						W.E = this;
						W.Upcoming.Add(u.Skip(1).ToList());
						break;
				}
			}
			else
			{
				var nextNextUpcoming = new List<List<object>>();
				foreach (var what in (List<object>)u.First()) {
					nextNextUpcoming.Add(((List<object>)what).ToList());
				}
				if (u.Count() > 1) {
					foreach (var next in nextNextUpcoming) {
						next.AddRange(((List<object>)u).Skip(1).ToList());
					}
				}
				nextUpcoming = nextNextUpcoming;
			}
		}
		this.Upcoming = nextUpcoming;
	}
}
public List<object> Listify(string regex)
{
	var stack = new Stack<object>();
	var buf = new List<object>();
	while (regex.Any())
	{
		switch (regex.First())
		{
			case '^':
				break;
			case '$':
				break;
			case '(':
				stack.Push(regex.First());
				break;
			case ')':
				var choices = new List<object>();
				var cur = new List<object>();
				while (!(stack.Peek() is char c) || c != '(')
				{
					var top = stack.Pop();
					if (top is char && (char)top == '|')
					{
						cur.Reverse();
						choices.Add(cur);
						cur = new List<object>();
					}
					else
					{
						cur.Add(top);
					}
				}
				stack.Pop();
				cur.Reverse();
				choices.Add(cur);
				choices.Reverse();
				stack.Push(choices);
				break;
			default:
				stack.Push(regex.First());
				break;
		}
		regex = regex.Substring(1);
	}
	var list = stack.ToList();
	list.Reverse();
	return list;
}
public void Print(Dictionary<(int X, int Y), Room> rooms)
{
var sb = new StringBuilder();
for (var y = rooms.Keys.Max(k => k.Y); y >= rooms.Keys.Min(k => k.Y); y--)
{
for (var x = rooms.Keys.Min(k => k.X); x <= rooms.Keys.Max(k => k.X); x++)
{
			if (x == 0 && y == 0)
			{
				sb.Append("¤");
				continue;
			}
			if (!rooms.ContainsKey((x, y)))
{
sb.Append("█");
continue;
}
var room = rooms[(x, y)];
if (room.N != null & room.E != null && room.S != null && room.W != null)
{
sb.Append("╬");
}
else if (room.N == null && room.E != null && room.S != null && room.W != null)
{
sb.Append("╦");
}
else if (room.N != null && room.E == null && room.S != null && room.W != null)
{
sb.Append("╣");
}
else if (room.N != null && room.E != null && room.S == null && room.W != null)
{
sb.Append("╩");
}
else if (room.N != null && room.E != null && room.S != null && room.W == null)
{
sb.Append("╠");
}
else if (room.S != null && room.W != null && room.N == null && room.E == null)
{
sb.Append("╗");
}
else if (room.N != null && room.W != null && room.E == null && room.S == null)
{
sb.Append("╝");
}
else if (room.N != null && room.E != null && room.S == null && room.W == null)
{
sb.Append("╚");
}
else if (room.S != null && room.E != null && room.N == null && room.W == null)
{
sb.Append("╔");
}
else if (room.S != null && room.N != null && room.E == null && room.W == null)
{
sb.Append("║");
}
else if (room.W != null && room.E != null && room.N == null && room.S == null)
{
sb.Append("═");
}
else
{
sb.Append("■");
}
}
sb.AppendLine();
}
Console.WriteLine(sb.ToString());
}
//Coped from The Internet
private class PathNode
{
	public int X;
	public int Y;
	public double F;
	public double G;
	public double H;
	public PathNode Parent;
}
private List<(int X, int Y)> BuildPath(Dictionary<(int X, int Y), Room> rooms, (int X, int Y) start, (int X, int Y) target)
{
	PathNode current = null;
	var openList = new List<PathNode>();
	var closedList = new List<PathNode>();
	int g = 0;

	// start by adding the original position to the open list
	openList.Add(new PathNode { X = start.X, Y = start.Y });

	while (openList.Count > 0)
	{
		// get the square with the lowest F score
		var lowest = openList.Min(l => l.F);
		current = openList.Where(l => l.F == lowest).OrderBy(f => f.Y).ThenBy(f => f.X).First();

		// add the current square to the closed list
		closedList.Add(current);
		// remove it from the open list
		openList.Remove(current);

		// if we added the destination to the closed list, we've found a path
		if (closedList.FirstOrDefault(l => l.X == target.X && l.Y == target.Y) != null)
			break;

		var adjacentSquares = GetWalkableAdjacentSquares(rooms[(current.X, current.Y)]);
		g++;

		foreach (var adjacentSquare in adjacentSquares)
		{
			// if this adjacent square is already in the closed list, ignore it
			if (closedList.FirstOrDefault(l => l.X == adjacentSquare.X
					&& l.Y == adjacentSquare.Y) != null)
				continue;

			// if it's not in the open list...
			if (openList.FirstOrDefault(l => l.X == adjacentSquare.X
					&& l.Y == adjacentSquare.Y) == null)
			{
				// compute its score, set the parent
				adjacentSquare.G = g;
				adjacentSquare.H = ComputeHScore(adjacentSquare.X, adjacentSquare.Y, target.X, target.Y);
				adjacentSquare.F = adjacentSquare.G + adjacentSquare.F;// + adjacentSquare.H;
				adjacentSquare.Parent = current;

				// and add it to the open list
				openList.Insert(0, adjacentSquare);
			}
			else
			{
				// test if using the current G score makes the adjacent square's F score
				// lower, if yes update the parent because it means it's a better path
				if (g + adjacentSquare.F + adjacentSquare.H < adjacentSquare.F)
				{
					adjacentSquare.G = g;
					adjacentSquare.F = adjacentSquare.G + adjacentSquare.F + adjacentSquare.H;
					adjacentSquare.Parent = current;
				}
			}
		}
	}

	// assume path was found; let's show it
	var bestpath = new List<(int X, int Y)>();
	if (current.X != target.X || current.Y != target.Y) return new List<(int X, int Y)>();
	while (current != null)
	{
		bestpath.Add((current.X, current.Y));
		current = current.Parent;
	}
	bestpath.Reverse();
	return bestpath;
}
List<PathNode> GetWalkableAdjacentSquares(Room room)
{
	var proposedLocations = new List<PathNode>()
			{
				room.N != null ? new PathNode { X = room.N.X, Y = room.N.Y } : null,
				room.E != null ? new PathNode { X = room.E.X, Y = room.E.Y } : null,
				room.S != null ? new PathNode { X = room.S.X, Y = room.S.Y } : null,
				room.W != null ? new PathNode { X = room.W.X, Y = room.W.Y } : null,
			};
	
	return proposedLocations.Where(l => l != null).ToList();
}
static int ComputeHScore(int x, int y, int targetX, int targetY)
{
	return Math.Abs(targetX - x) + Math.Abs(targetY - y);
}
public int Distance(string travelled)
{
	int x = 0;
	int y = 0;
	var total = 0;
	foreach (var character in travelled)
	{
		switch (character)
		{
			case 'N':
				y++;
				total++;
				break;
			case 'E':
				total++;
				x++;
				break;
			case 'S':
				total++;
				y--;
				break;
			case 'W':
				total++;
				x--;
				break;
		}
	}
	if (x == 0 && y == 0) return 0;
	return total;
}