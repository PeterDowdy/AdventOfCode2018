<Query Kind="Program" />

bool cancel(char a, char b) {
	return Math.Abs(a-b) == 32;
}
void Main() {
	var input = File.ReadAllText($"{Path.GetDirectoryName (Util.CurrentQueryPath)}/5.txt");
	var parseStack = new Stack<char>();
	for (var i = 0; i < input.Length; i++)
	{
		if (parseStack.Any() && cancel(parseStack.Peek(), input[i]))
		{
			parseStack.Pop();
		}
		else
		{
			parseStack.Push(input[i]);
		}
	}
	parseStack.Count.Dump();
}