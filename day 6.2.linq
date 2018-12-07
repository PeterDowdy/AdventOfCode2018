<Query Kind="Program" />

public int ManhattanDistance((int X, int Y) first, (int X, int Y) other) {
	return Math.Abs(first.X-other.X) + Math.Abs(first.Y-other.Y);
}
void Main()
{
	var input = File.ReadAllText($"{Path.GetDirectoryName(Util.CurrentQueryPath)}/6.txt");
	var coordinates = new List<(int X, int Y)>();
	foreach (var line in input.Replace("\r", "").Split('\n').Where(x => !string.IsNullOrEmpty(x)))
	{
		var coordinateValues = line.Split(' ');
		coordinates.Add((X:int.Parse(coordinateValues[0].Replace(",","")), Y:int.Parse(coordinateValues[1])));
	};

var iterations = 0;
	var searchBox = (
	UpperLeftX: (coordinates.Sum(c => c.X) / coordinates.Count)-1,
	UpperLeftY: (coordinates.Sum(c => c.Y) / coordinates.Count)-1,
	LowerRightX: (coordinates.Sum(c => c.X) / coordinates.Count)+1,
	LowerRightY: (coordinates.Sum(c => c.Y) / coordinates.Count) + 1
	);
	var coords = new Dictionary<(int X, int Y), bool>();
	for (var x = searchBox.UpperLeftX; x <= searchBox.LowerRightX; x++)
	{
		for (var y = searchBox.UpperLeftY; y <= searchBox.LowerRightY; y++)
		{
			if (coordinates.Sum(c => ManhattanDistance(c, (x, y))) < 10000) coords[(x, y)] = true;
		}
	}
	var expand = (MinusX: true, MinusY: true, PlusX: true, PlusY: true);
	var lastIterDistances = (MinusX: 0, MinusY: 0, PlusX: 0, PlusY: 0);
	do
	{
		iterations++;
		if (expand.MinusY) { 
		var thisMax = lastIterDistances.MinusY;
		var thisMin = lastIterDistances.MinusY;
			for (var i = searchBox.UpperLeftX; i <= searchBox.LowerRightX; i++)
			{
				var manhattanDistance = coordinates.Sum(c => ManhattanDistance(c, (i, searchBox.UpperLeftY - 1)));
				thisMax = Math.Max(thisMax, manhattanDistance);
				thisMin = Math.Min(thisMin, manhattanDistance);
				if (manhattanDistance < 10000) coords[(i, searchBox.UpperLeftY-1)] = true;
			}
			if (thisMin > 10000) expand.MinusY = false;
			else
			{
				lastIterDistances.MinusY = thisMax;
				searchBox.UpperLeftY--;
			}
		}
		if (expand.MinusX)
		{
			var thisMax = lastIterDistances.MinusX;
			var thisMin = lastIterDistances.MinusX;
			for (var i = searchBox.UpperLeftY; i <= searchBox.LowerRightY; i++)
			{
				var manhattanDistance = coordinates.Sum(c => ManhattanDistance(c, (searchBox.UpperLeftX - 1, i)));
				thisMax = Math.Max(thisMax, manhattanDistance);
				thisMin = Math.Min(thisMin, manhattanDistance);
				if (manhattanDistance < 10000) coords[(searchBox.UpperLeftX - 1,i)] = true;
			}
			if (thisMin > 10000) expand.MinusX = false;
			else
			{
				lastIterDistances.MinusX = thisMax;
				searchBox.UpperLeftX--;
			}
		}
		if (expand.PlusY)
		{
			var thisMax = lastIterDistances.PlusY;
			var thisMin = lastIterDistances.PlusY;
			for (var i = searchBox.UpperLeftX; i <= searchBox.LowerRightX; i++)
			{
				var manhattanDistance = coordinates.Sum(c => ManhattanDistance(c, (i, searchBox.LowerRightY + 1)));
				thisMax = Math.Max(thisMax, manhattanDistance);
				thisMin = Math.Min(thisMin, manhattanDistance);
				if (manhattanDistance < 10000) coords[(i, searchBox.LowerRightY + 1)] = true;
			}
			if (thisMin > 10000) expand.PlusY = false;
			else
			{
				lastIterDistances.PlusY = thisMax;
				searchBox.LowerRightY++;
			}
		}
		if (expand.PlusX)
		{
			var thisMax = lastIterDistances.PlusX;
			var thisMin = lastIterDistances.PlusX;
			for (var i = searchBox.UpperLeftY; i <= searchBox.LowerRightY; i++)
			{
				var manhattanDistance = coordinates.Sum(c => ManhattanDistance(c, (searchBox.LowerRightX + 1, i)));
				thisMax = Math.Max(thisMax, manhattanDistance);
				thisMin = Math.Min(thisMin, manhattanDistance);
				if (manhattanDistance < 10000) coords[(searchBox.LowerRightX + 1, i)] = true;
			}
			if (thisMin > 10000) expand.PlusX = false;
			else
			{
				lastIterDistances.PlusX = thisMax;
				searchBox.LowerRightX++;
			}
		}
	} while (expand.PlusX || expand.PlusY || expand.MinusX || expand.MinusY);
	Console.WriteLine();
	coords.Count.Dump();
}