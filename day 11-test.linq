<Query Kind="Program" />

public void Main()
{
	var serial = 71;
	var dimension = 300;
	var cells = new int[dimension,dimension];
	for (var y = 0; y < dimension; y++) {
		for (var x = 0; x < dimension; x++) {
			cells[y,x] = ((((x+11)*(y+1)+serial)*(x+11) / 100) % 10) -5;
		}
	}
	cells[79-1, 122-1].Dump();
	cells[196-1, 217-1].Dump();
	cells[153-1, 101-1].Dump();
	//Print(cells);
}
public void Print(int[,] cells)
{
	for (var y = 0; y < cells.GetLength(0); y++)
	{
		for (var x = 0; x < cells.GetLength(1); x++)
		{
			Console.Write(cells[y,x].ToString().PadLeft(5));
		}
		Console.Write("\n\n\n\n");
	}
}
