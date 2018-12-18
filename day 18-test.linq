<Query Kind="Program" />

void Main()
{
	var input = @".#.#...|#.
.....#|##|
.|..|...#.
..|#.....#
#.#|||#|#|
...#.||...
.|....|...
||...#|.#|
|.||||..|.
...#.|..|.".Split(new[] { Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).ToList();
	var cells = new Dictionary<(int X, int Y), (char Initial, char Next)>();
	for (var y = 0; y < input.Count; y++) {
		for (var x = 0; x < input[0].Length; x++) {
			cells[(y,x)] = (input[x][y], ' ');
		}
	}
	var minutes = 0;
	Console.WriteLine("Initial state");
	Print(cells);
	do {
	var keys = cells.Keys.ToList();
		foreach (var key in keys) {
			var adjacents = new List<(char Initial, char Next)>();
			var cell = cells[key];
			if (cells.ContainsKey((key.X + 1, key.Y))) adjacents.Add(cells[(key.X + 1, key.Y)]);
			if (cells.ContainsKey((key.X + 1, key.Y + 1))) adjacents.Add(cells[(key.X + 1, key.Y + 1)]);
			if (cells.ContainsKey((key.X, key.Y + 1))) adjacents.Add(cells[(key.X, key.Y + 1)]);
			if (cells.ContainsKey((key.X - 1, key.Y + 1))) adjacents.Add(cells[(key.X - 1, key.Y + 1)]);
			if (cells.ContainsKey((key.X - 1, key.Y))) adjacents.Add(cells[(key.X - 1, key.Y)]);
			if (cells.ContainsKey((key.X - 1, key.Y - 1))) adjacents.Add(cells[(key.X - 1, key.Y - 1)]);
			if (cells.ContainsKey((key.X, key.Y - 1))) adjacents.Add(cells[(key.X, key.Y - 1)]);
			if (cells.ContainsKey((key.X + 1, key.Y - 1))) adjacents.Add(cells[(key.X + 1, key.Y - 1)]);
			switch (cell.Initial)
			{
				case '.':
				if (adjacents.Count(a => a.Initial == '|') > 2) {
					cell.Next = '|';
				} else {
					cell.Next = '.';
				}
					break;
				case '|':
					if (adjacents.Count(a => a.Initial == '#') > 2)
					{
						cell.Next = '#';
					}
					else
					{
						cell.Next = '|';
					}
					break;
				case '#':
					if (adjacents.Any(a => a.Initial == '#') && adjacents.Any(a => a.Initial == '|'))
					{
						cell.Next = '#';
					}
					else
					{
						cell.Next = '.';
					}
					break;
			}
			cells[key] = cell;
		}
		foreach (var key in keys)
		{
			var cell = cells[key];
			cell.Initial = cell.Next;
			cells[key] = cell;
		}
		minutes++;
		Console.WriteLine($"After minute {minutes}");
		Print(cells);
	} while (minutes < 10);
	(cells.Values.Count(v => v.Initial == '|')*cells.Values.Count(v => v.Initial == '#')).Dump();
}
private void Print(Dictionary<(int X, int Y), (char Initial, char Next)> cells) {
var buf = new StringBuilder();
	for (var y = cells.Keys.Min(k => k.Y); y <= cells.Keys.Max(k => k.Y); y++) {
		for (var x = cells.Keys.Min(k => k.X); x <= cells.Keys.Max(k => k.X); x++) {
			buf.Append(cells[(x,y)].Initial);
		}
		buf.AppendLine();
	}
	Console.WriteLine(buf.ToString());
}
// Define other methods and classes here
