<Query Kind="Program" />

//this is the actual solution
public class GameOfLifeRule {
	bool[] Predicate;
	public bool Outcome;
	public GameOfLifeRule(string s)
	{
		Predicate = s.Substring(0, 5).Select(s2 => s2 == '#').ToArray();
		Outcome = s.Last() == '#';
	}
	public bool Match(bool[] value)
	{
		return value[0] == Predicate[0] && value[1] == Predicate[1] && value[2] == Predicate[2] && value[3] == Predicate[3] && value[4] == Predicate[4];
	}
}
public class Pot
{
	public int Index { get; set; }
	public bool Plant { get; set; }
	public Pot Left { get; set; }
	public Pot Right { get; set; }
	public bool NextGeneration { get; set; }
}
void Main()
{
	var generations = 50000000000;
	var SeenStructures = new Dictionary<string,bool>();
	var input = File.ReadAllText($"{Path.GetDirectoryName(Util.CurrentQueryPath)}/12.txt");
	var lines = input.Replace("\r", "").Split('\n');
	var initialStateString = lines[0].Replace("initial state: ", "");
	var initialState = new Pot { Index = 0, Plant = initialStateString[0] == '#' };
	initialState.Left = new Pot { Index = initialState.Index - 1, Plant = false, Right = initialState };
	initialState.Left.Left = new Pot { Index = initialState.Index - 2, Plant = false, Right = initialState.Left };
	initialState.Left.Left.Left = new Pot { Index = initialState.Index - 3, Plant = false, Right = initialState.Left.Left };
	initialState.Left.Left.Left.Left = new Pot { Index = initialState.Index - 4, Plant = false, Right = initialState.Left.Left.Left };
	var lastState = initialState;
	for (var i = 1; i < initialStateString.Length; i++)
	{
		var newNode = new Pot { Index = i, Plant = initialStateString[i] == '#', Left = lastState };
		lastState.Right = newNode;
		lastState = newNode;
	}
	lastState.Right = new Pot { Index = lastState.Index + 1, Plant = false, Left = lastState };
	lastState.Right.Right = new Pot { Index = lastState.Index + 2, Plant = false, Left = lastState.Right };
	lastState.Right.Right.Right = new Pot { Index = lastState.Index + 3, Plant = false, Left = lastState.Right.Right };
	lastState.Right.Right.Right.Right = new Pot { Index = lastState.Index + 4, Plant = false, Left = lastState.Right.Right.Right };
	var rules = lines.Skip(2).Select(x => new GameOfLifeRule(x));
	var min = initialState;
	while (min.Left != null) min = min.Left;
	var max = initialState;
	while (max.Right != null) max = max.Right;
	while (!max.Plant) max = max.Left;
	var buf = "";
	while (!min.Plant) min = min.Right;
	do
	{
		buf += min.Plant ? "#" : ".";
		min = min.Right;
	} while (min != max);
	buf += max.Plant ? "#" : ".";
	Console.WriteLine(buf);
	SeenStructures[buf] = true;
	do
	{
		min = initialState;
		while (min.Left.Left != null)
		{
			var subSection = new[] { min.Left.Left.Plant, min.Left.Plant, min.Plant, min.Right.Plant, min.Right.Right.Plant };
			var applicableRule = rules.FirstOrDefault(r => r.Match(subSection));
			if (applicableRule != null)
			{
				min.NextGeneration = applicableRule.Outcome;
			}
			else min.NextGeneration = false;
			min = min.Left;
		}
		while (min.Left != null) min = min.Left;
		min.Left = new Pot { Index = min.Index - 1, Right = min, Plant = false };
		min = min.Left;
		max = initialState;
		while (max.Right.Right != null)
		{
			var subSection = new[] { max.Left.Left.Plant, max.Left.Plant, max.Plant, max.Right.Plant, max.Right.Right.Plant };
			var applicableRule = rules.FirstOrDefault(r => r.Match(subSection));
			if (applicableRule != null)
			{
				max.NextGeneration = applicableRule.Outcome;
			}
			else max.NextGeneration = false;
			max = max.Right;
		}
		while (max.Right != null) max = max.Right;
		max.Right = new Pot { Index = max.Index + 1, Left = max, Plant = false };
		max = max.Right;
		do
		{
			min.Plant = min.NextGeneration;
			min = min.Right;
		} while (min != max);
		max.Plant = max.NextGeneration;
		generations--;
		min = initialState;
		while (min.Left != null) min = min.Left;
		max = initialState;
		while (max.Right != null) max = max.Right;
		while (!max.Plant) max = max.Left;
		while (!min.Plant) min = min.Right;
		buf = "";
		do
		{
			buf += min.Plant ? "#" : ".";
			min = min.Right;
		} while (min != max);
		buf += max.Plant ? "#" : ".";
		Console.WriteLine(buf);
		if (SeenStructures.ContainsKey(buf))
		{
			CalculateRemaining(min.Index, generations, initialState).Dump();
			break;
		}
		else SeenStructures[buf] = true;
	} while (generations > 0);
}
public long CalculateRemaining(long leftestPlant, long generationsRemaining, Pot initialState)
{
	var generationGap = generationsRemaining-leftestPlant;
	var min = initialState;
	while (min.Left != null) min = min.Left;
	var max = initialState;
	while (max.Right != null) max = max.Right;
	long plantCount = 0;
	do
	{
		plantCount += min.Plant ? (min.Index+generationGap) : 0;
		min = min.Right;
	} while (min != max);
	return plantCount;
}//2100000000061