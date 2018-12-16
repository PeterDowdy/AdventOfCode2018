<Query Kind="Program" />

void Main()
{
	var program = File.ReadAllText($"{Path.GetDirectoryName(Util.CurrentQueryPath)}/16.txt");
	var opCodeGuesses = program.Split(new[] { "\r\n\r\n\r\n" }, StringSplitOptions.None)[0].Replace("\r","").Replace("\n\n","\n").Split('\n').ToArray();
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
	var totalThatMatchThreeOrMore = 0;
	foreach (var testCase in testCases) {
	var opCodesMatched = 0;
		var input = new Register(testCase.Before);
		if (new Register(testCase.Before).ADDR(testCase.OpCode).Equals(new Register(testCase.After))) opCodesMatched++;
		if (new Register(testCase.Before).ADDI(testCase.OpCode).Equals(new Register(testCase.After))) opCodesMatched++;
		if (new Register(testCase.Before).MULR(testCase.OpCode).Equals(new Register(testCase.After))) opCodesMatched++;
		if (new Register(testCase.Before).MULI(testCase.OpCode).Equals(new Register(testCase.After))) opCodesMatched++;
		if (new Register(testCase.Before).BANR(testCase.OpCode).Equals(new Register(testCase.After))) opCodesMatched++;
		if (new Register(testCase.Before).BANI(testCase.OpCode).Equals(new Register(testCase.After))) opCodesMatched++;
		if (new Register(testCase.Before).BORR(testCase.OpCode).Equals(new Register(testCase.After))) opCodesMatched++;
		if (new Register(testCase.Before).BORI(testCase.OpCode).Equals(new Register(testCase.After))) opCodesMatched++;
		if (new Register(testCase.Before).SETR(testCase.OpCode).Equals(new Register(testCase.After))) opCodesMatched++;
		if (new Register(testCase.Before).SETI(testCase.OpCode).Equals(new Register(testCase.After))) opCodesMatched++;
		if (new Register(testCase.Before).GTIR(testCase.OpCode).Equals(new Register(testCase.After))) opCodesMatched++;
		if (new Register(testCase.Before).GTRI(testCase.OpCode).Equals(new Register(testCase.After))) opCodesMatched++;
		if (new Register(testCase.Before).GTRR(testCase.OpCode).Equals(new Register(testCase.After))) opCodesMatched++;
		if (new Register(testCase.Before).EQIR(testCase.OpCode).Equals(new Register(testCase.After))) opCodesMatched++;
		if (new Register(testCase.Before).EQRI(testCase.OpCode).Equals(new Register(testCase.After))) opCodesMatched++;
		if (new Register(testCase.Before).EQRR(testCase.OpCode).Equals(new Register(testCase.After))) opCodesMatched++;
		if (opCodesMatched >= 3) totalThatMatchThreeOrMore++;
	}
	totalThatMatchThreeOrMore.Dump();
}
public class Register {
	public bool Equals(Register other) {
		return _registers[0] == other.Registers[0] &&
		_registers[1] == other.Registers[1] &&
		_registers[2] == other.Registers[2] &&
		_registers[3] == other.Registers[3];
	}
	private int[] _registers = new int[] { 0, 0, 0, 0 };
	public int[] Registers => _registers;
	public Register(int[] registers) {
		for (var i = 0; i < registers.Length; i++) {
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
