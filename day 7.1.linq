<Query Kind="Program" />


void Main()
{
	var input = File.ReadAllText($"{Path.GetDirectoryName(Util.CurrentQueryPath)}/7.txt");
	var steps = input.Replace("\r", "").Split('\n').Select(i => i.Substring(5,1)).ToList();
	steps.AddRange(input.Replace("\r", "").Split('\n').Select(i => i.Substring(36,1)));
	steps = steps.Distinct().ToList();
	var stepChains = input.Replace("\r", "").Split('\n').Select(i => (Step: i.Substring(36,1), Prereq: i.Substring(5,1)));
	var stepRequirements = steps.ToDictionary(s => s, s => stepChains.Where(sc=> sc.Step == s).Select(sc => sc.Prereq).ToList());
	var chain = new List<string>();
	do
	{
		var orphans = stepRequirements.Where(s => !s.Value.Any() || s.Value.All(s2 => chain.Contains(s2))).ToList();
		var batman = orphans.OrderBy(o => o.Key).First();
		stepRequirements.Remove(batman.Key);
		stepRequirements = stepRequirements.ToDictionary(s => s.Key, s => s.Value.Where(v => v != batman.Key).ToList());
		chain.Add(batman.Key);
	} while (stepRequirements.Any());
	string.Join("", chain.Distinct().Select(c => c)).Dump();
}