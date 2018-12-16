<Query Kind="Program" />

void Main()
{
	var clayTiles = new Dictionary<(int X, int Y),bool>();
	var testCoordinates = File.ReadLines($"{Path.GetDirectoryName(Util.CurrentQueryPath)}/17.txt").SelectMany(x =>
	{
		var coordinates = new List<(int X, int Y)>();
		var values = x.Split(',');
		var xValues = values.Single(v => v.Contains("x")).Trim().Substring(2);
		var yValues = values.Single(v => v.Contains("y")).Trim().Substring(2);
		if (xValues.Contains("..")) {
			var yValue = int.Parse(yValues);
			for (var xStart = int.Parse(xValues.Split(new[] { ".." }, StringSplitOptions.None)[0]); xStart <= int.Parse(xValues.Split(new[] { ".." }, StringSplitOptions.None)[1]); xStart++)
			{
				clayTiles[(xStart, yValue)] = true;
				coordinates.Add((xStart, yValue));
			}
		}
		else
		{
			var xValue = int.Parse(xValues);
			for (var yStart = int.Parse(yValues.Split(new[] { ".." }, StringSplitOptions.None)[0]); yStart <= int.Parse(yValues.Split(new[] { ".." }, StringSplitOptions.None)[1]); yStart++)
			{
				clayTiles[(xValue, yStart)] = true;
				coordinates.Add((xValue, yStart));
			}
		}
		return coordinates;
	}).ToArray();
	MinY = testCoordinates.Min(c => c.Y);
	MinX = testCoordinates.Min(c => c.X)-1;
	var soilCells = new SoilCell[testCoordinates.Max(c => c.Y)+1,testCoordinates.Max(c => c.X)+2];
	for (var y = 0; y < soilCells.GetLength(0); y++) {
		for (var x = 0; x < soilCells.GetLength(1); x++) {
			soilCells[y,x] = new SoilCell { HasWater = false, HadWater = false, Clay = clayTiles.ContainsKey((x,y))};
		}
	}
	soilCells[0,500].Spring = true;
	//Print(soilCells);

	FlowDown((500, 0), soilCells);
	Print(soilCells);

	var waterCount = 0;
	for (var y = MinY; y < soilCells.GetLength(0); y++)
	{
		for (var x = MinX; x < soilCells.GetLength(1); x++)
		{
			if (soilCells[y,x].HasWater) waterCount++;
		}
	}
	waterCount.Dump();
}
private static int MinY;
private static int MinX;
private void Print(SoilCell[,] cells)
{
	var sb = new StringBuilder();
	for (var y = MinY; y < cells.GetLength(0); y++)
	{
		for (var x = MinX; x < cells.GetLength(1); x++)
		{
			if (cells[y, x].Clay) sb.Append("#");
			else if (cells[y, x].HasWater) sb.Append("~");
			else if (cells[y, x].HadWater) sb.Append("|");
			else if (x == 500 && y == 0) sb.Append("+");
			else sb.Append(".");
		}
		sb.Append(Environment.NewLine);
	}
	Console.Write(sb.ToString());
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
	public bool HasWater {get; set;}
	public bool HadWater {get; set;}
	public bool Clay {get; set;}
	public bool Spring {get; set;}
}
// Define other methods and classes here