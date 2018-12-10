<Query Kind="Program" />

public class Point
{
	public Point(string point)
	{
		var tokens = point.Replace(" velocity=<", ",").Replace("position=<", "").Replace(">", "").Split(',');
		Position = (int.Parse(tokens[0]), int.Parse(tokens[1]));
		Velocity = (int.Parse(tokens[2]), int.Parse(tokens[3]));
	}
	public (int X, int Y) Position { get; set; }
	public (int X, int Y) Velocity { get; set; }
	public void Process() {
		Position = (Position.X + Velocity.X, Position.Y + Velocity.Y);
	}
}
public void Main()
{
	var points = File.ReadAllText($"{Path.GetDirectoryName(Util.CurrentQueryPath)}/10.txt").Replace("\r", "").Split('\n').Select(x => new Point(x)).ToList();
	for (; ; )
	{
		Draw(points);
		points.ForEach(x => x.Process());
	}
}
void Draw(IEnumerable<Point> points)
{
	var boundingBox = (xMin: points.Select(p => p.Position.X).Min(), yMin: points.Select(p => p.Position.Y).Min(), xMax: points.Select(p => p.Position.X).Max(), yMax: points.Select(p => p.Position.Y).Max());
	if ((boundingBox.yMax - boundingBox.yMin) > 256 || (boundingBox.xMax-boundingBox.xMin) > 256) {
		return;
	} 
	var charArray = new char[1 + boundingBox.yMax - boundingBox.yMin][];
	var buf = new StringBuilder();
	for (var y = boundingBox.yMin; y <= boundingBox.yMax; y++)
	{
		charArray[y - boundingBox.yMin] = new char[1 + boundingBox.xMax - boundingBox.xMin];
		for (var x = boundingBox.xMin; x <= boundingBox.xMax; x++)
		{
			charArray[y - boundingBox.yMin][x - boundingBox.xMin] = '.';
		}
	}
	foreach (var point in points)
	{
		charArray[point.Position.Y - boundingBox.yMin][point.Position.X - boundingBox.xMin] = '#';
	}
	for (var y = 0; y < charArray.Length; y++)
	{
		buf.Append(charArray[y]);
		buf.Append('\n');
	}
	Console.WriteLine(buf.ToString());
}