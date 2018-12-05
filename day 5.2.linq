<Query Kind="Program" />

bool cancel(char a, char b) {
	return Math.Abs(a-b) == 32;
}
void Main() {
var polymerExclusions = "abcdefghijklmnopqrstuvwxyz".ToDictionary(k => k, k =>
{
	var input = File.ReadAllText($"{Path.GetDirectoryName(Util.CurrentQueryPath)}/5.txt");
	var parseStack = new Stack<char>();
	var curLength = input.Length;
	curLength = input.Length;
	while (input.Any())
	{
		if (input.First() == k || (input.First() + 32) == k) { input = input.Substring(1);}
		else if (parseStack.Any() && cancel(parseStack.Peek(), input.First()))
		{
			parseStack.Pop();
			input = input.Substring(1);
		}
		else
		{
			parseStack.Push(input.First());
			input = input.Substring(1);
		}
	}
	return parseStack.Count;
});
polymerExclusions.Values.Min().Dump();
}