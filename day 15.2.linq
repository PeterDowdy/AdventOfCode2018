<Query Kind="Program" />

public static class DebugConsole
{
	private static bool Debug = false;
	public static void Write(string s)
	{
		if (Debug) Console.Write(s);
	}
	public static void WriteLine(string s)
	{
		if (Debug) Console.WriteLine(s);
	}
	public static void WriteLine()
	{
		if (Debug) Console.WriteLine();
	}
}
void Main()
{
	var battle = File.ReadAllText($"{Path.GetDirectoryName(Util.CurrentQueryPath)}/15.txt");
	Console.WriteLine();
	var attackPower = 24; //numbers derived from wild guessing
	Battlefield battlefield = null;
	do
	{
		battlefield = new Battlefield(battle, attackPower);
		//Console.WriteLine("FIGHT");
		//Console.WriteLine(battlefield.ToString());
		while (battlefield.BattleRages())
		{
			if (battlefield.AnElfDied) {
			attackPower += 6;
			break;}
			var combatantsInReadingOrder = battlefield.NextTurn;
			foreach (var combatant in combatantsInReadingOrder)
			{
				if (!combatant.Dead)
				{
					battlefield.TakeTurn(combatant);
				}
			}
		}
	} while (battlefield.AnElfDied);
	Console.WriteLine($"Elves ({attackPower}) win with {attackPower}: {battlefield.BodyCount.Dump()}");
	do
	{
		attackPower--;
		battlefield = new Battlefield(battle, attackPower);
		//Console.WriteLine("FIGHT");
		//Console.WriteLine(battlefield.ToString());
		while (battlefield.BattleRages())
		{
			if (battlefield.AnElfDied)
			{
				Console.WriteLine($"Elves ({attackPower}) first lost with {attackPower}");
				break;
			}
			var combatantsInReadingOrder = battlefield.NextTurn;
			foreach (var combatant in combatantsInReadingOrder)
			{
				if (!combatant.Dead)
				{
					battlefield.TakeTurn(combatant);
				}
			}
		}
		if (battlefield.AnElfDied) break;
		Console.WriteLine($"Elves ({attackPower}) win: {battlefield.BodyCount.Dump()}");
	} while (true);
	//Console.WriteLine("Battle over");
}
public class Battlefield
{
	private int ElfAttackPower = 3;
	public bool AnElfDied = false;
	private int TurnCount { get; set; } = 0;
	private (int X, int Y, bool Wall, Occupant Occupant)[,] Cells;
	private List<Occupant> Occupants = new List<Occupant>();
	public Battlefield(string field, int elfAttackPower)
	{
		ElfAttackPower = elfAttackPower;
		var rows = field.Replace("\r", "").Split('\n').ToList();
		Cells = new(int X, int Y, bool wall, Occupant occupant)[rows.Count(), rows.First().Length];
		for (var y = 0; y < Cells.GetLength(0); y++)
		{
			for (var x = 0; x < Cells.GetLength(1); x++)
			{
				Cells[y, x] = (x, y, rows[y][x] == '#', rows[y][x] == 'E' || rows[y][x] == 'G' ? new Occupant(rows[y][x], x, y) : null);
				if (Cells[y, x].Occupant != null) Occupants.Add(Cells[y, x].Occupant);
			}
		}
	}
	public bool BattleRages()
	{
		return Occupants.Any(x => x.Type == 'G') && Occupants.Any(x => x.Type == 'E');
	}
	public List<Occupant> NextTurn
	{
		get
		{
			TurnCount++;
			return Occupants.OrderBy(o => o.Coordinates.Y).ThenBy(o => o.Coordinates.X).ToList();
		}
	}
	public void TakeTurn(Occupant occupant)
	{
		DebugConsole.WriteLine($"({occupant.Coordinates.X},{occupant.Coordinates.Y})...");
		var nearestEnemySquares = Occupants.Where(o => o.Type != occupant.Type).SelectMany(x =>
		{
			var neighbouringSquares = new List<(int X, int Y, bool Wall, Occupant Occupant)>();
			if (!Cells[x.Coordinates.Y - 1, x.Coordinates.X].Wall && Cells[x.Coordinates.Y - 1, x.Coordinates.X].Occupant == null || Cells[x.Coordinates.Y - 1, x.Coordinates.X].Occupant == occupant) neighbouringSquares.Add(Cells[x.Coordinates.Y - 1, x.Coordinates.X]);
			if (!Cells[x.Coordinates.Y, x.Coordinates.X + 1].Wall && Cells[x.Coordinates.Y, x.Coordinates.X + 1].Occupant == null || Cells[x.Coordinates.Y, x.Coordinates.X + 1].Occupant == occupant) neighbouringSquares.Add(Cells[x.Coordinates.Y, x.Coordinates.X + 1]);
			if (!Cells[x.Coordinates.Y + 1, x.Coordinates.X].Wall && Cells[x.Coordinates.Y + 1, x.Coordinates.X].Occupant == null || Cells[x.Coordinates.Y + 1, x.Coordinates.X].Occupant == occupant) neighbouringSquares.Add(Cells[x.Coordinates.Y + 1, x.Coordinates.X]);
			if (!Cells[x.Coordinates.Y, x.Coordinates.X - 1].Wall && Cells[x.Coordinates.Y, x.Coordinates.X - 1].Occupant == null || Cells[x.Coordinates.Y, x.Coordinates.X - 1].Occupant == occupant) neighbouringSquares.Add(Cells[x.Coordinates.Y, x.Coordinates.X - 1]);
			return neighbouringSquares;
		}).ToList();
		var startPath = new List<(int X, int Y)>();
		startPath.Add((occupant.Coordinates));
		var foundPaths = nearestEnemySquares.Select(x => BuildPath((occupant.Coordinates.X, occupant.Coordinates.Y), (x.X,x.Y))).Where(x => x.Any()).ToList();
		var closest = foundPaths.OrderBy(o => o.Count).ThenBy(o => o.Last().Y).ThenBy(o => o.Last().X).FirstOrDefault();
		if (closest == null) return;
		if (closest.Count == 1) Attack(occupant);
		else if (closest.Count == 2) //move and attack
		{
			Cells[occupant.Coordinates.Y, occupant.Coordinates.X].Occupant = null;
			occupant.Coordinates = closest.Skip(1).First();
			DebugConsole.WriteLine($"Move: {occupant.Coordinates.X}, {occupant.Coordinates.Y}");
			Cells[occupant.Coordinates.Y, occupant.Coordinates.X].Occupant = occupant;
			Attack(occupant);
		}
		else //move without attacking
		{
			Cells[occupant.Coordinates.Y, occupant.Coordinates.X].Occupant = null;
			occupant.Coordinates = closest.Skip(1).First();
			DebugConsole.WriteLine($"Move: {occupant.Coordinates.X}, {occupant.Coordinates.Y}");
			Cells[occupant.Coordinates.Y, occupant.Coordinates.X].Occupant = occupant;
		}
	}
	public void Attack(Occupant attacker)
	{
		var neighbouringSquares = new List<(int X, int Y, bool Wall, Occupant Occupant)>();
		var enemy = new[] { Cells[attacker.Coordinates.Y - 1, attacker.Coordinates.X],
		Cells[attacker.Coordinates.Y, attacker.Coordinates.X - 1],Cells[attacker.Coordinates.Y, attacker.Coordinates.X + 1],
		Cells[attacker.Coordinates.Y + 1, attacker.Coordinates.X] }
		.Where(x => x.Occupant != null && x.Occupant.Type != attacker.Type).OrderBy(x => x.Occupant.Hp).FirstOrDefault().Occupant;

		if (enemy != null)
		{
			DebugConsole.WriteLine($"ATTACK {enemy.Coordinates.X},{enemy.Coordinates.Y}");
			if (enemy.Type == 'G') enemy.Hp -= ElfAttackPower; else enemy.Hp -= 3;
			if (enemy.Hp <= 0)
			{
				if (enemy.Type == 'E') AnElfDied = true;
				Occupants.Remove(enemy);
				Cells[enemy.Coordinates.Y, enemy.Coordinates.X].Occupant = null;
				enemy.Dead = true;
			}
		}
	}
	public int BodyCount => (TurnCount-1) * Occupants.Select(o => o.Hp).Sum();
	//Coped from The Internet
	private class PathNode {
		public int X;
		public int Y;
		public double F;
		public double G;
		public double H;
		public PathNode Parent;
	}
	private List<(int X, int Y)> BuildPath((int X, int Y) start, (int X, int Y) target)
	{
		PathNode current = null;
		var openList = new List<PathNode>();
            var closedList = new List<PathNode>();
            int g = 0;

		// start by adding the original position to the open list
		openList.Add(new PathNode {X = start.X, Y = start.Y});

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

                var adjacentSquares = GetWalkableAdjacentSquares(current.X, current.Y);
                g++;

                foreach(var adjacentSquare in adjacentSquares)
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
					//adjacentSquare.H = ComputeHScore(adjacentSquare.X, adjacentSquare.Y, target.X, target.Y);
					adjacentSquare.F = adjacentSquare.G + adjacentSquare.F;// + adjacentSquare.H;
					adjacentSquare.Parent = current;

					// and add it to the open list
					openList.Insert(0, adjacentSquare);
				}
				else
				{
					// test if using the current G score makes the adjacent square's F score
					// lower, if yes update the parent because it means it's a better path
					if (g + adjacentSquare.F/*adjacentSquare.H*/ < adjacentSquare.F)
					{
						adjacentSquare.G = g;
						adjacentSquare.F = adjacentSquare.G + adjacentSquare.F;// + adjacentSquare.H;
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
	List<PathNode> GetWalkableAdjacentSquares(int x, int y)
	{
		var proposedLocations = new List<PathNode>()
			{
				new PathNode{X= x, Y= y - 1},
				new PathNode{X= x-1, Y= y},
				new PathNode{X= x+1, Y= y},
				new PathNode{X= x, Y= y + 1}
			};

		return proposedLocations.Where(l => !Cells[l.Y, l.X].Wall && Cells[l.Y, l.X].Occupant == null).ToList();
	}

	static int ComputeHScore(int x, int y, int targetX, int targetY)
	{
		return Math.Abs(targetX - x) + Math.Abs(targetY - y);
	}
	public override string ToString()
	{
		var buf = "";
		for (var y = 0; y < Cells.GetLength(0); y++)
		{
			for (var x = 0; x < Cells.GetLength(1); x++)
			{
				buf += (Cells[y, x].Occupant?.Type ?? (Cells[y, x].Wall ? '#' : '.'));
			}
			buf += "\r\n";
		}
		return buf;
	}
}
public class Occupant
{
	public bool Dead { get; set; }
	public (int X, int Y) Coordinates { get; set; }
	public char Type { get; set; }
	public int Hp { get; set; } = 200;
	public Occupant(char input, int x, int y)
	{
		Type = input;
		Coordinates = (x, y);
	}
}