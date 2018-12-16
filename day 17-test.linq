<Query Kind="Program" />

void Main()
{
	var testCoordinates = @"x=495, y=2..7
y=7, x=495..501
x=501, y=3..7
x=498, y=2..4
x=506, y=1..2
x=498, y=10..13
x=504, y=10..13
y=13, x=498..504".Split(new[] { "\r\n"}, StringSplitOptions.RemoveEmptyEntries).SelectMany(x =>
	{
		var coordinates = new List<(int X, int Y)>();
		var values = x.Split(',',' ');
		var xValues = values.Single(v => v.Contains("x")).Substring(2);
		var yValues = values.Single(v => v.Contains("y")).Substring(2);
		if (xValues.Contains("..")) {
		var yValue = int.Parse(yValues);
			for (var xStart = int.Parse(xValues.Split(new[] { ".." }, StringSplitOptions.None)[0]); xStart <= int.Parse(xValues.Split(new[] { ".." }, StringSplitOptions.None)[1]); xStart++) {
				coordinates.Add((xStart, yValue));
			}
		}
		else
		{
			var xValue = int.Parse(xValues);
			for (var yStart = int.Parse(yValues.Split(new[] { ".." }, StringSplitOptions.None)[0]); yStart <= int.Parse(yValues.Split(new[] { ".." }, StringSplitOptions.None)[1]); yStart++)
			{
				coordinates.Add((xValue, yStart));
			}
		}
		return coordinates;
	});
	Depth = testCoordinates.Max(c => c.Y)+1;
	Left = testCoordinates.Min(c => c.X);
	Right = testCoordinates.Max(c => c.X)+1;
	var soilCells = new SoilCell[testCoordinates.Max(c => c.Y)+1,testCoordinates.Max(c => c.X)+2];
	for (var y = 0; y < soilCells.GetLength(0); y++) {
		for (var x = 0; x < soilCells.GetLength(1); x++) {
			soilCells[y,x] = new SoilCell { HasWater = false, HadWater = false, Clay = testCoordinates.Any(c => c.Y == y && c.X == x) };
		}
	}
	soilCells[0,500].Spring = true;
	Print(soilCells);

	FlowDown((500, 0), soilCells);
	Print(soilCells);

	var waterCount = 0;
	for (var y = 0; y < soilCells.GetLength(0); y++)
	{
		for (var x = 0; x < soilCells.GetLength(1); x++)
		{
			if (soilCells[y,x].HadWater || soilCells[y,x].HasWater) waterCount++;
		}
	}
	waterCount.Dump();
}
private static int Depth;
private static int Left;
private static int Right;
private void Print(SoilCell[,] cells) {
	var buf = "";
	for (var y = 0; y < Depth; y++) {
		for (var x = Left - 1; x < Right + 1; x++) {
			if (cells[y,x].Clay) buf += "#";
			else if (cells[y,x].HasWater) buf += "~";
			else if (cells[y,x].HadWater) buf += "|";
			else if (x == 500 && y == 0) buf += "+";
			else buf += ".";
		}
		buf += "\r\n";
	}
	Console.WriteLine(buf);
}
private void FlowDown((int X, int Y) start, SoilCell[,] cells)
{
	if (start.Y + 1 == cells.GetLength(0)) return;
	var down = cells[start.Y + 1, start.X];
	if (down.HadWater && !down.HasWater) return;
	if (!down.HasWater && !down.Clay)
	{
		down.HadWater = true;
		//Print(cells);
		FlowDown((start.X, start.Y + 1), cells);
		return;
	}
	if (start.X != 0)
	{
		//Print(cells);
		Splash((start.X, start.Y), cells);
	}
}
private void Splash((int X, int Y) impact, SoilCell[,] cells)
{
	cells[impact.Y,impact.X].HadWater = true;
	var left = impact;
	var stopLeft = false;
	var flowDownLeft = false;
	var right = impact;
	var stopRight = false;
	var flowDownRight = false;
	for (; ; )
	{
		var leftCell = cells[left.Y, left.X - 1];
		var leftBelowCell = cells[left.Y + 1, left.X - 1];
		if (!stopLeft && !leftBelowCell.HasWater && !leftBelowCell.Clay)
		{
			leftCell.HadWater = true;
			flowDownLeft = true;
			//Print(cells);
			FlowDown((left.X - 1, left.Y), cells);
		}
		if (!stopLeft && leftCell.Clay)
		{
			stopLeft = true;
		}
		else if (!stopLeft && !flowDownLeft)
		{
			leftCell.HadWater = true;
			left = (left.X-1, left.Y);
		}
		var rightCell = cells[right.Y, right.X + 1];
		var rightBelowCell = cells[right.Y+1, right.X+1];
		if (!stopRight && !rightBelowCell.HasWater && !rightBelowCell.Clay)
		{
			rightCell.HadWater = true;
			flowDownRight = true;
			//Print(cells);
			FlowDown((right.X+1, right.Y), cells);
		}
		else if (!stopRight && rightCell.Clay)
		{
			stopRight = true;
		}
		else if (!stopRight && !flowDownRight)
		{
			rightCell.HadWater = true;
			right = (right.X + 1, right.Y);
		}
		if (stopLeft && stopRight) {
			for (var x = left.X; x <= right.X; x++)
			{
				cells[impact.Y, x].HadWater = true;
				cells[impact.Y, x].HasWater = true;
			}
			//Print(cells);
			Splash((impact.X, impact.Y-1), cells);
			break;
		}
		if (flowDownLeft && flowDownRight) {
			break;
		}
		if (flowDownLeft && stopRight) {
			break;
		}
		if (stopLeft && flowDownRight) {
			break;
		}
		//Print(cells);
	}		
}
public class SoilCell {
	public bool HasWater { get; set; }
	public bool HadWater { get; set; }
	public bool Clay { get; set; }
	public bool Spring { get; set; }
}
// Define other methods and classes here