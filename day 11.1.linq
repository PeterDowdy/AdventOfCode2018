<Query Kind="Program" />

public void Main()
{
	var serial = 3214;
	var dimension = 300;
	var cells = new int[dimension, dimension];
	for (var y = 0; y < dimension; y++)
	{
		for (var x = 0; x < dimension; x++)
		{
			cells[y, x] = ((((x + 11) * (y + 1) + serial) * (x + 11) / 100) % 10) - 5;
		}
	}
	var maxValue = 0;
	var maxCoord = (-1,-1);
	for (var y = 0; y < dimension - 2; y++)
	{
		for (var x = 0; x < dimension - 2; x++)
		{
			var newMax = cells[y, x] + cells[y, x + 1] + cells[y, x + 2] +
			cells[y + 1, x] + cells[y + 1, x + 1] + cells[y + 1, x + 2] +
			cells[y + 2, x] + cells[y + 2, x + 1] + cells[y + 2, x + 2];
			if (newMax > maxValue) {
				maxCoord = (x+1,y+1);
				maxValue = newMax;
			}
		}
	}
	maxCoord.Dump();
}