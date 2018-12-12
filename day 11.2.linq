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
	var searchWindow = 300;
	var maxSoFar = (X: -1, Y: -1, Window: searchWindow, Power: int.MinValue);
	while (searchWindow >= 1)
	{
		for (var y = 0; y < dimension - searchWindow + 1; y++)
		{
			for (var x = 0; x < dimension - searchWindow + 1; x++)
			{
				var power = 0;
				for (var yStart = y; yStart < y + searchWindow; yStart++)
				{
					for (var xStart = x; xStart < x + searchWindow; xStart++)
					{
						power += cells[yStart, xStart];
					}
				}
				if (power > maxSoFar.Power) maxSoFar = (x, y, searchWindow, power);
			}
		} searchWindow--;
	}
	maxSoFar.Dump();
}