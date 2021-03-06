<Query Kind="Program" />

void Main()
{
	var input = File.ReadLines($"{Path.GetDirectoryName(Util.CurrentQueryPath)}/18.txt").ToList();
	var cells = new Dictionary<(int X, int Y), (char Initial, char Next)>();
	for (var y = 0; y < input.Count; y++) {
		for (var x = 0; x < input[0].Length; x++) {
			cells[(y,x)] = (input[x][y], ' ');
		}
	}
	var minutes = 0;
	var sequence = new List<string>();
	var scores = new List<int>();
	var fastForward = false;
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
		if (!fastForward)
		{
			var nextSequence = MakeString(cells);
			if (sequence.Contains(nextSequence))
			{
				var lastSeen = sequence.IndexOf(nextSequence) + 1;
				var minuteGap = minutes - lastSeen;
				var minutesRemaining = 1000000000 - minutes;
				minutes += minuteGap * (minutesRemaining / minuteGap);
				fastForward = true;
			}
			else
			{
				sequence.Add(nextSequence);
			}
		}
		Print(cells);
		Console.WriteLine($"Completed minute {minutes}");
	} while (minutes < 1000000000);
	(cells.Values.Count(v => v.Initial == '|') * cells.Values.Count(v => v.Initial == '#')).Dump();
}
private string MakeString(Dictionary<(int X, int Y), (char Initial, char Next)> cells)
{
	var buf = new StringBuilder();
	for (var y = cells.Keys.Min(k => k.Y); y <= cells.Keys.Max(k => k.Y); y++)
	{
		for (var x = cells.Keys.Min(k => k.X); x <= cells.Keys.Max(k => k.X); x++)
		{
			buf.Append(cells[(x, y)].Initial);
		}
		buf.AppendLine();
	}
	return buf.ToString();
}private void Print(Dictionary<(int X, int Y), (char Initial, char Next)> cells) {
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
