<Query Kind="Program" />

void Main()
{
	var program = File.ReadAllText($"{Path.GetDirectoryName(Util.CurrentQueryPath)}/16.txt");
	var opCodeGuesses = program.Split(new[] { "\r\n\r\n\r\n" }, StringSplitOptions.None)[0].Replace("\r","").Replace("\n\n","\n").Split('\n').ToArray();
	var opCodePossibilities = new Dictionary<string, List<int>>
	{
		["addr"] = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 },
		["addi"] = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 },
		["mulr"] = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 },
		["muli"] = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 },
		["banr"] = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 },
		["bani"] = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 },
		["borr"] = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 },
		["bori"] = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 },
		["setr"] = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 },
		["seti"] = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 },
		["gtir"] = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 },
		["gtri"] = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 },
		["gtrr"] = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 },
		["eqir"] = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 },
		["eqri"] = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 },
		["eqrr"] = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 },
	};
	var testCases = new List<(int[] OpCode, int[] Before, int[] After)>();
	(int[] Registers, int[] Before, int[] After) nextTestCase = (new int[4], new int[4], new int[4]);
	for (var i = 0; i < opCodeGuesses.Length; i++) {
		if (i % 3 == 0) {
			nextTestCase.Before = opCodeGuesses[i].Replace("Before: [", "").Replace("]","").Split(',').Select(x => x.Trim()).Select(int.Parse).ToArray();
		}
		if (i % 3 == 1)
		{
			nextTestCase.Registers = opCodeGuesses[i].Split(' ').Select(x => x.Trim()).Select(int.Parse).ToArray();
		}
		if (i % 3 == 2)
		{
			nextTestCase.After = opCodeGuesses[i].Replace("After:  [", "").Replace("]", "").Split(',').Select(x => x.Trim()).Select(int.Parse).ToArray();
			testCases.Add(nextTestCase);
			nextTestCase = (new int[4], new int[4], new int[4]);
		}
	}
	foreach (var testCase in testCases) {
		var input = new Register(testCase.Before);
		if (!new Register(testCase.Before).ADDR(testCase.OpCode).Equals(new Register(testCase.After))) opCodePossibilities["addr"].Remove(testCase.OpCode[0]);
		if (!new Register(testCase.Before).ADDI(testCase.OpCode).Equals(new Register(testCase.After))) opCodePossibilities["addi"].Remove(testCase.OpCode[0]);
		if (!new Register(testCase.Before).MULR(testCase.OpCode).Equals(new Register(testCase.After))) opCodePossibilities["mulr"].Remove(testCase.OpCode[0]);
		if (!new Register(testCase.Before).MULI(testCase.OpCode).Equals(new Register(testCase.After))) opCodePossibilities["muli"].Remove(testCase.OpCode[0]);
		if (!new Register(testCase.Before).BANR(testCase.OpCode).Equals(new Register(testCase.After))) opCodePossibilities["banr"].Remove(testCase.OpCode[0]);
		if (!new Register(testCase.Before).BANI(testCase.OpCode).Equals(new Register(testCase.After))) opCodePossibilities["bani"].Remove(testCase.OpCode[0]);
		if (!new Register(testCase.Before).BORR(testCase.OpCode).Equals(new Register(testCase.After))) opCodePossibilities["borr"].Remove(testCase.OpCode[0]);
		if (!new Register(testCase.Before).BORI(testCase.OpCode).Equals(new Register(testCase.After))) opCodePossibilities["bori"].Remove(testCase.OpCode[0]);
		if (!new Register(testCase.Before).SETR(testCase.OpCode).Equals(new Register(testCase.After))) opCodePossibilities["setr"].Remove(testCase.OpCode[0]);
		if (!new Register(testCase.Before).SETI(testCase.OpCode).Equals(new Register(testCase.After))) opCodePossibilities["seti"].Remove(testCase.OpCode[0]);
		if (!new Register(testCase.Before).GTIR(testCase.OpCode).Equals(new Register(testCase.After))) opCodePossibilities["gtir"].Remove(testCase.OpCode[0]);
		if (!new Register(testCase.Before).GTRI(testCase.OpCode).Equals(new Register(testCase.After))) opCodePossibilities["gtri"].Remove(testCase.OpCode[0]);
		if (!new Register(testCase.Before).GTRR(testCase.OpCode).Equals(new Register(testCase.After))) opCodePossibilities["gtrr"].Remove(testCase.OpCode[0]);
		if (!new Register(testCase.Before).EQIR(testCase.OpCode).Equals(new Register(testCase.After))) opCodePossibilities["eqir"].Remove(testCase.OpCode[0]);
		if (!new Register(testCase.Before).EQRI(testCase.OpCode).Equals(new Register(testCase.After))) opCodePossibilities["eqri"].Remove(testCase.OpCode[0]);
		if (!new Register(testCase.Before).EQRR(testCase.OpCode).Equals(new Register(testCase.After))) opCodePossibilities["eqrr"].Remove(testCase.OpCode[0]);
	}
	do
	{
		foreach (var identifiedCode in opCodePossibilities.Where(kvp => kvp.Value.Count == 1))
		{
			foreach (var eliminationCandidate in opCodePossibilities.Where(kvp => kvp.Value.Count != 1))
			{
				eliminationCandidate.Value.Remove(identifiedCode.Value.Single());
			}
		}
	} while (opCodePossibilities.Sum(kvp => kvp.Value.Count) > 16);
	var discoveredCodes = opCodePossibilities.ToDictionary(kvp => kvp.Value.Single(), kvp => kvp.Key);
	//if this doesn't work, your problem is gonna suck
	var code = program.Split(new[] { "\r\n\r\n\r\n" }, StringSplitOptions.None)[1].Replace("\r", "").Split('\n').Where(x => !string.IsNullOrEmpty(x)).Select(u => u.Split(' ').Select(x => x.Trim()).Select(int.Parse).ToArray()).ToArray();
	var registers = new Register(new[] { 0, 0, 0, 0 });
	foreach (var instruction in code) {
		switch (discoveredCodes[instruction[0]])
		{
			case "addr": 
			registers.ADDR(instruction);
				break;
			case "addi":
				registers.ADDI(instruction); break;
			case "mulr":
				registers.MULR(instruction); break;
			case "muli":
				registers.MULI(instruction); break;
			case "banr":
				registers.BANR(instruction); break;
			case "bani":
				registers.BANI(instruction); break;
			case "borr":
				registers.BORR(instruction); break;
			case "bori":
				registers.BORI(instruction); break;
			case "setr":
				registers.SETR(instruction); break;
			case "seti":
				registers.SETI(instruction); break;
			case "gtir":
				registers.GTIR(instruction); break;
			case "gtri":
				registers.GTRI(instruction); break;
			case "gtrr":
				registers.GTRR(instruction); break;
			case "eqir":
				registers.EQIR(instruction); break;
			case "eqri":
				registers.EQRI(instruction); break;
			case "eqrr":
				registers.EQRR(instruction); break;
		}
	}
	registers.Registers.Dump();
}
public class Register
{
	public bool Equals(Register other)
	{
		return _registers[0] == other.Registers[0] &&
		_registers[1] == other.Registers[1] &&
		_registers[2] == other.Registers[2] &&
		_registers[3] == other.Registers[3];
	}
	private int[] _registers = new int[] { 0, 0, 0, 0 };
	public int[] Registers => _registers;
	public Register(int[] registers)
	{
		for (var i = 0; i < registers.Length; i++)
		{
			_registers[i] = registers[i];
		}
	}
	public Register ADDR(int[] command)
	{
		_registers[command[3]] = _registers[command[1]] + _registers[command[2]];
		return this;
	}
	public Register ADDI(int[] command)
	{
		_registers[command[3]] = _registers[command[1]] + command[2];
		return this;
	}
	public Register MULR(int[] command)
	{
		_registers[command[3]] = _registers[command[1]] * _registers[command[2]];
		return this;
	}
	public Register MULI(int[] command)
	{
		_registers[command[3]] = _registers[command[1]] * command[2];
		return this;
	}
	public Register BANR(int[] command)
	{
		_registers[command[3]] = _registers[command[1]] & _registers[command[2]];
		return this;
	}
	public Register BANI(int[] command)
	{
		_registers[command[3]] = _registers[command[1]] & command[2];
		return this;
	}
	public Register BORR(int[] command)
	{
		_registers[command[3]] = _registers[command[1]] | _registers[command[2]];
		return this;
	}
	public Register BORI(int[] command)
	{
		_registers[command[3]] = _registers[command[1]] | command[2];
		return this;
	}
	public Register SETR(int[] command)
	{
		_registers[command[3]] = _registers[command[1]];
		return this;
	}
	public Register SETI(int[] command)
	{
		_registers[command[3]] = command[1];
		return this;
	}
	public Register GTIR(int[] command)
	{
		_registers[command[3]] = command[1] > _registers[command[2]] ? 1 : 0;
		return this;
	}
	public Register GTRI(int[] command)
	{
		_registers[command[3]] = _registers[command[1]] > command[2] ? 1 : 0;
		return this;
	}
	public Register GTRR(int[] command)
	{
		_registers[command[3]] = _registers[command[1]] > _registers[command[2]] ? 1 : 0;
		return this;
	}
	public Register EQIR(int[] command)
	{
		_registers[command[3]] = command[1] == _registers[command[2]] ? 1 : 0;
		return this;
	}
	public Register EQRI(int[] command)
	{
		_registers[command[3]] = _registers[command[1]] == command[2] ? 1 : 0;
		return this;
	}
	public Register EQRR(int[] command)
	{
		_registers[command[3]] = _registers[command[1]] == _registers[command[2]] ? 1 : 0;
		return this;
	}
}

// Define other methods and classes here
