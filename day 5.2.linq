<Query Kind="Program" />

bool cancel(char a, char b) {
	return Math.Abs(a-b) == 32;
}
void Main()
{
	var input = File.ReadAllText($"{Path.GetDirectoryName(Util.CurrentQueryPath)}/5.txt");
	var polymerExclusions = "abcdefghijklmnopqrstuvwxyz".ToDictionary(k => k, k =>
	{
		var parseStack = new Stack<char>();
		for (var i = 0; i < input.Length; i++)
		{
			if (input[i] == k || (input[i] + 32) == k) { continue; }
			else if (parseStack.Any() && cancel(parseStack.Peek(), input[i]))
			{
				parseStack.Pop();
			}
			else
			{
				parseStack.Push(input[i]);
			}
		}
		return parseStack.Count;
	});
	polymerExclusions.Values.Min().Dump();
}