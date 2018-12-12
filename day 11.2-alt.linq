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
	BinaryFuzzyDecompose(1,1,300,cells, int.MinValue).OrderByDescending(b => b.Value).First().Dump();
}
public IEnumerable<(int X, int Y, int Size, int Value)> BinaryFuzzyDecompose(int xCoord, int yCoord, int searchSize, int[,] cells, int maxSoFar)
{
	var result = new List<(int X, int Y, int Size, int Value)>();
	var sizeOfThis = 0;
	for (var y = 0; y < searchSize; y++)
	{
		for (var x = 0; x < searchSize; x++)
		{
			sizeOfThis += cells[y,x];
		}
	}
	var subAreaOneSize = 0;
	for (var y = 0; y < searchSize-1; y++)
	{
		for (var x = 0; x < searchSize-1; x++)
		{
			subAreaOneSize += cells[y, x];
		}
	}
	var subAreaTwoSize = 0;
	for (var y = 1; y < searchSize; y++)
	{
		for (var x = 0; x < searchSize-1; x++)
		{
			subAreaTwoSize += cells[y, x];
		}
	}
	var subAreaThreeSize = 0;
	for (var y = 0; y < searchSize-1; y++)
	{
		for (var x = 1; x < searchSize; x++)
		{
			subAreaThreeSize += cells[y, x];
		}
	}
	var subAreaFourSize = 0;
	for (var y = 1; y < searchSize; y++)
	{
		for (var x = 1; x < searchSize; x++)
		{
			subAreaFourSize += cells[y, x];
		}
	}
	if (sizeOfThis > maxSoFar) result.Add((xCoord, yCoord, cells.GetLength(0), sizeOfThis));
	var subMax = Math.Max(Math.Max(Math.Max(subAreaOneSize, subAreaTwoSize), subAreaThreeSize), subAreaFourSize);
	if (subMax < sizeOfThis) return new[] { (xCoord, yCoord, cells.GetLength(0), sizeOfThis) };
	if (subMax == subAreaOneSize) result.AddRange(BinaryFuzzyDecompose(xCoord,yCoord,searchSize-1,cells, subAreaOneSize));
	if (subMax == subAreaTwoSize) result.AddRange(BinaryFuzzyDecompose(xCoord+1,yCoord,searchSize-1,cells, subAreaTwoSize));
	if (subMax == subAreaThreeSize) result.AddRange( BinaryFuzzyDecompose(xCoord,yCoord+1,searchSize-1,cells, subAreaThreeSize));
	if (subMax == subAreaFourSize) result.AddRange( BinaryFuzzyDecompose(xCoord+1,yCoord+1,searchSize-1,cells, subAreaFourSize));
	return result;
}