<Query Kind="Program" />

void Main()
{
	var program = new[] {
	"#ip 0",
"seti 5 0 1",
"seti 6 0 2",
"addi 0 1 0",
"addr 1 2 3",
"setr 1 0 0",
"seti 8 0 4",
"seti 9 0 5"};
	var registers = new Register(new[] { 0, 0, 0, 0, 0, 0 });
	var programCounter = int.Parse(program.First().Replace("#ip ", ""));
	program = program.Skip(1).ToArray();
	for(;;) {
		var nextLine = registers.Registers[programCounter];
		if (nextLine >= program.Count()) {
			nextLine.Dump();
			break;
		}
		var instruction = program[nextLine];
		var instructionParts = instruction.Split(' ');
		var opCode = new List<int>() { 0 };
		opCode.AddRange(instructionParts.Skip(1).Select(int.Parse));
		switch (instructionParts.First())
		{
			case "addr":
				registers.ADDR(opCode.ToArray());
				break;
			case "addi":
				registers.ADDI(opCode.ToArray());
				break;
			case "mulr":
				registers.MULR(opCode.ToArray());
				break;
			case "muli":
				registers.MULI(opCode.ToArray());
				break;
			case "banr":
				registers.BANR(opCode.ToArray());
				break;
			case "bani":
				registers.BANI(opCode.ToArray());
				break;
			case "borr":
				registers.BORR(opCode.ToArray());
				break;
			case "bori":
				registers.BORI(opCode.ToArray());
				break;
			case "setr":
				registers.SETR(opCode.ToArray());
				break;
			case "seti":
				registers.SETI(opCode.ToArray());
				break;
			case "gtir":
				registers.GTIR(opCode.ToArray());
				break;
			case "gtri":
				registers.GTRI(opCode.ToArray());
				break;
			case "gtrr":
				registers.GTRR(opCode.ToArray());
				break;
			case "eqir":
				registers.EQIR(opCode.ToArray());
				break;
			case "eqri":
				registers.EQRI(opCode.ToArray());
				break;
			case "eqrr":
				registers.EQRR(opCode.ToArray());
				break;
		}
		registers.Registers[programCounter]++;
	}
}
public class Register
{
	public bool Equals(Register other)
	{
		return _registers[0] == other.Registers[0] &&
		_registers[1] == other.Registers[1] &&
		_registers[2] == other.Registers[2] &&
		_registers[3] == other.Registers[3] &&
		_registers[4] == other.Registers[4] &&
		_registers[5] == other.Registers[5];
	}
	private int[] _registers = new int[] { 0, 0, 0, 0, 0, 0 };
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