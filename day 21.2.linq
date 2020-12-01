<Query Kind="Program" />


void Main()
{
	var register1 = 16134795+1;
	long fewestExecutionsSoFar = long.MaxValue;
	long bestRegisterValueSoFar = long.MaxValue;
	var breaks = new List<int>();
	while (register1 >= 0)
	{
		var program = File.ReadAllLines($"{Path.GetDirectoryName(Util.CurrentQueryPath)}/21.txt");
		var executions = 0;
		var registers = new Register(new[] { register1, 0, 0, 0, 0, 0 });
		var programCounter = int.Parse(program.First().Replace("#ip ", ""));
		program = program.Skip(1).ToArray();
		var compiled = program.Select(Compile).ToArray();
		//Console.WriteLine($"                         [{string.Join(",", new[] { 0, 1, 2, 3, 4, 5 }.Select(r => r.ToString().PadLeft(18)))}]");
		for (; ; )
		{
			var nextLine = registers.Registers[programCounter];
			if (nextLine == 30) {
				if (breaks.Contains(registers.Registers[3])) {
					Console.WriteLine(breaks.Last());
				}
				breaks.Add(registers.Registers[3]);
			}
			if (nextLine >= program.Count())
			{
				break;
			}
			var instruction = program[nextLine];
			//var start = registers.Registers.Select(r => r.ToString()).ToList();
			compiled[nextLine](registers);
			executions++;
			if (executions > fewestExecutionsSoFar || executions == long.MaxValue) break;
			registers.Registers[programCounter]++;
			//var end = start.Select((s, idx) => (s != registers.Registers[idx].ToString() ? $"{s}=>{registers.Registers[idx].ToString()}" : registers.Registers[idx].ToString()).PadLeft(18));
			//Console.WriteLine($"[{nextLine.ToString().PadLeft(2)}] {instruction.PadLeft(18)}: [{string.Join(",", end)}]");
		}
		if (executions <= fewestExecutionsSoFar)
		{
			Console.WriteLine($"0 is {register1}, stopped after {executions}");
			fewestExecutionsSoFar = executions;
			bestRegisterValueSoFar = register1;
		}
		register1--;
	}
}

public Action<Register> Compile(string instruction)
{
	var instructionParts = instruction.Split(' ');
	var opCode = new List<int>() { 0 };
	if (instructionParts.Length == 4) opCode.AddRange(instructionParts.Skip(1).Select(int.Parse));
	switch (instructionParts.First())
	{
		case "innerloop":
			return registers =>
			{
				//mulr 5 3 4
				//eqrr 4 1 4
				//addr 4 2 2
				//addi 2 1 2
				//addr 5 0 0
				//addi 3 1 3
				//gtrr 3 1 4 
				//addr 2 4 2
				//seti 2 1 2
				for (; ; )
				{
					if (registers.Registers[1] % registers.Registers[5] == 0) registers.Registers[0] += registers.Registers[5];
					registers.Registers[3] = registers.Registers[1] + 1;
					if (registers.Registers[3] > registers.Registers[1]) break;
				}
			};
		case "nop": return registers => { };
		case "addr":
			return registers => registers.ADDR(opCode.ToArray());
		case "addi":
			return registers => registers.ADDI(opCode.ToArray());
		case "mulr":
			return registers => registers.MULR(opCode.ToArray());
		case "muli":
			return registers => registers.MULI(opCode.ToArray());
		case "banr":
			return registers => registers.BANR(opCode.ToArray());
		case "bani":
			return registers => registers.BANI(opCode.ToArray());
		case "borr":
			return registers => registers.BORR(opCode.ToArray());
		case "bori":
			return registers => registers.BORI(opCode.ToArray());
		case "setr":
			return registers => registers.SETR(opCode.ToArray());
		case "seti":
			return registers => registers.SETI(opCode.ToArray());
		case "gtir":
			return registers => registers.GTIR(opCode.ToArray());
		case "gtri":
			return registers => registers.GTRI(opCode.ToArray());
		case "gtrr":
			return registers => registers.GTRR(opCode.ToArray());
		case "eqir":
			return registers => registers.EQIR(opCode.ToArray());
		case "eqri":
			return registers => registers.EQRI(opCode.ToArray());
		case "eqrr":
			return registers => registers.EQRR(opCode.ToArray());
	}
	return null;
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
		lowest3SoFar = Math.Min(lowest3SoFar, _registers[3]);
		return this;
	}
	public static int lowest3SoFar = int.MaxValue;
}
// Define other methods and classes here