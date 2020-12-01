<Query Kind="Program" />

void Main()
{
	var target = (X: 10, Y: 10);
	var depth = 510;
	var cells = new Dictionary<(int X, int Y), int>();
	for (var x = 0; x <= target.X; x++) {
		for (var y = 0; y <= target.Y; y++)
		{
			if (x == 0 && y == 0) cells[(x, y)] = (0 + depth) % 20183;
			else if (x == target.X && y == target.Y) cells[(x, y)] = (0 + depth) % 20183;
			else if (x == 0) { cells[(x, y)] = (48271 * y + depth) % 20183; }
			else if (y == 0) { cells[(x, y)] = (16807 * x + depth) % 20183; }
			else { cells[(x,y)] = (cells[(x-1,y)]*cells[(x,y-1)] + depth) % 20183;}
		}
	}
	var danger = cells.Where(c => c.Key.X >= 0 && c.Key.X <= target.X && c.Key.Y >= 0 && c.Key.Y <= target.Y).Select(c => c.Value % 3).Sum();
	danger.Dump();
}
// Define other methods and classes here
