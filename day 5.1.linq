<Query Kind="Program" />

bool cancel(char a, char b) {
	return Math.Abs(a-b) == 32;
}
void Main() {
	var input = File.ReadAllText($"{Path.GetDirectoryName (Util.CurrentQueryPath)}/5.txt");
	input.Length.Dump();
	var parseStack = new Stack<char>();
	var curLength = input.Length;
		curLength = input.Length;
	while (input.Any())
	{
		Console.WriteLine($"Length of {input.Length}");
		if (parseStack.Any() && cancel(parseStack.Peek(), input.First()))
			{
				parseStack.Pop();
				input = input.Substring(1);
			}
			else
			{
				parseStack.Push(input.First());
  		}
		}
		input = new string(parseStack.ToArray());
	input.Dump();
	input.Length.Dump();
}