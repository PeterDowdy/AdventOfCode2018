<Query Kind="Program" />

public class Point
{
	public Point(string point) {
		var tokens = point.Replace(" velocity=<", ",").Replace("position=<", "").Replace(">", "").Split(',');
		Position = (int.Parse(tokens[0]), int.Parse(tokens[1]));
		Velocity = (int.Parse(tokens[2]), int.Parse(tokens[3]));
	}
	public (int X, int Y) Position { get; set; }
	public (int X, int Y) Velocity {get; set;}
}
public void Main()
{
	var points = @"position=< 9,  1> velocity=< 0,  2>
position=< 7,  0> velocity=<-1,  0>
position=< 3, -2> velocity=<-1,  1>
position=< 6, 10> velocity=<-2, -1>
position=< 2, -4> velocity=< 2,  2>
position=<-6, 10> velocity=< 2, -2>
position=< 1,  8> velocity=< 1, -1>
position=< 1,  7> velocity=< 1,  0>
position=<-3, 11> velocity=< 1, -2>
position=< 7,  6> velocity=<-1, -1>
position=<-2,  3> velocity=< 1,  0>
position=<-4,  3> velocity=< 2,  0>
position=<10, -3> velocity=<-1,  1>
position=< 5, 11> velocity=< 1, -2>
position=< 4,  7> velocity=< 0, -1>
position=< 8, -2> velocity=< 0,  1>
position=<15,  0> velocity=<-2,  0>
position=< 1,  6> velocity=< 1,  0>
position=< 8,  9> velocity=< 0, -1>
position=< 3,  3> velocity=<-1,  1>
position=< 0,  5> velocity=< 0, -1>
position=<-2,  2> velocity=< 2,  0>
position=< 5, -2> velocity=< 1,  2>
position=< 1,  4> velocity=< 2,  1>
position=<-2,  7> velocity=< 2, -2>
position=< 3,  6> velocity=<-1, -1>
position=< 5,  0> velocity=< 1,  0>
position=<-6,  0> velocity=< 2,  0>
position=< 5,  9> velocity=< 1, -2>
position=<14,  7> velocity=<-2,  0>
position=<-3,  6> velocity=< 2, -1>".Replace("\r","").Split('\n').Select(x => new Point(x)).ToList();
 for(;;) {
 	Draw(points);
	Process(points);
 }
}
void Process(IEnumerable<Point> points) {
	foreach (var point in points) {
		point.Position = (point.Position.X + point.Velocity.X, point.Position.Y + point.Velocity.Y);
	}
}
void Draw(IEnumerable<Point> points)
{
	var boundingBox = (xMin: points.Select(p => p.Position.X).Min(), yMin: points.Select(p => p.Position.Y).Min(), xMax: points.Select(p => p.Position.X).Max(), yMax: points.Select(p => p.Position.Y).Max());
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